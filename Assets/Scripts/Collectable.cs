using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private bool canCollect;
    void Start()
    {
        if(this.gameObject.name == ObjectSpawner.instance.collactableObjectName)
        {
            gameObject.transform.tag = "Collectable";
            GameManager.instance.maxCollectable++;
            GameManager.instance.onGameOver += CannotCollect;
            GameManager.instance.onBombExploaded += CanCollect;
        }
    }

    
    public void Collect()
    {
        if (canCollect)
        {
            Debug.Log("Topladýn");
            Instantiate(GameManager.instance._particle, transform.position, Quaternion.identity);
            SoundManager.instance.PlayMusic(Soundlar.Collect);
            GameManager.instance.currentCollectable++;
            Destroy(gameObject);
        }
    }

    private void CanCollect()
    {
        canCollect = true;
    }

    private void CannotCollect()
    {
        canCollect = false;
    }
}
