using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void ChangeLevelTo(int x)
    {
        string sceneToLoad = "level" + (x - 1);
        SceneManager.LoadScene(sceneToLoad);
    }
}
