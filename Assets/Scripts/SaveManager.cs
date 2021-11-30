using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetString("CurrentLevel1", Helper.Serialize(LevelManager.activeLevel.ToString()));
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("CurrentLevel1"))
        {
            LevelManager.activeLevel = int.Parse(Helper.Deserialize(PlayerPrefs.GetString("CurrentLevel1")));
        }
    }
}
