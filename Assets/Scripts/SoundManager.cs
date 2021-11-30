using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Soundlar
{
    Explosion,
    Collect,
    Lose,
    Win
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public List<AudioSource> sounds = new List<AudioSource>();

    private Soundlar soundlar;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            sounds.Add(transform.GetChild(i).GetComponent<AudioSource>());
        }
    }

   

    public void PlayMusic(Soundlar soundfx)
    {
        soundlar = soundfx;
        SoundsIndexes();
    }

    private void SoundsIndexes()
    {
        switch (soundlar)
        {
            case Soundlar.Explosion:
                sounds[1].Play();
                break;
            case Soundlar.Collect:
                sounds[2].Play();
                break;
            case Soundlar.Lose:
                sounds[3].Play();
                break;
            case Soundlar.Win:
                sounds[4].Play();
                break;
            default:
                break;
        }
    }

}
