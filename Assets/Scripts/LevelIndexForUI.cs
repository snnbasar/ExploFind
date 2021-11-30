using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIndexForUI : MonoBehaviour
{
    private int levelIndex;
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        levelIndex = int.Parse(gameObject.name) - 1;
        if (levelIndex <= LevelManager.activeLevel)
        {
            button.interactable = true;
        }
        else
            button.interactable = false;
    }

}
