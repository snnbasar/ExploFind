using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Cinemachine;

public class Bomb : MonoBehaviour
{

    private Rigidbody rb;
    private CinemachineImpulseSource impulse;
    [SerializeField] private float delay = 3f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float force = 700f;
    [SerializeField] private float havayaFirlatma = 3;

    [SerializeField] private GameObject explosionEffect;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        impulse = GetComponent<CinemachineImpulseSource>();
        GameManager.instance.onStartGame += ActivateBomb; //register Bomb to onStartGame event
    }

    public void ActivateBomb()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        Ignite();
    }

    private async void Ignite()
    {
        SoundManager.instance.PlayMusic(Soundlar.Explosion);
        await Task.Delay((int)(delay * 1000));
        await Task.WhenAny(Explode());
        GameManager.instance.SlowDownTime();
        Destroy(this.gameObject);
    }

    private async Task Explode()
    {
        Vector3 pos = new Vector3(1, 0, 1);
        pos = Vector3.Scale(pos, transform.position);
        Instantiate(explosionEffect, pos, Quaternion.identity);
        Instantiate(explosionEffect, pos * 2, Quaternion.identity);
        Instantiate(explosionEffect, pos * -2, Quaternion.identity);
        impulse.GenerateImpulse();
        GameManager.instance.BombExploaded();
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObjects in colliders)
        {
            Rigidbody objRb = nearbyObjects.GetComponent<Rigidbody>();
            if(objRb != null)
            {
                objRb.AddExplosionForce(force, transform.position + -Vector3.up * havayaFirlatma, radius);
            }
        }
        await Task.Yield();
    }
}
