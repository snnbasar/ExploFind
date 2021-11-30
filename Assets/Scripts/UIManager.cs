using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private TextMeshProUGUI remainingTime;
    [SerializeField] private TextMeshProUGUI remainingCollect;
    [SerializeField] private GameObject collectImage;
    [SerializeField] private Transform CollectingItems;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject missionSucceded;
    [SerializeField] private GameObject missionFailed;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }
    private void Start()
    {
        startGameButton.SetActive(false);
        CollectingItems.gameObject.SetActive(false);
        collectImage.SetActive(false);
        missionSucceded.SetActive(false);
        missionFailed.SetActive(false);
        GameManager.instance.onBombExploaded += ActivateCollectHud;
        GameManager.instance.onGameOver += DeactivateCollectHud;
        GameManager.instance.onGameOver += ActivateMissionFailedMenu;
        GameManager.instance.onMissionCompleted += DeactivateCollectHud;
        GameManager.instance.onMissionCompleted += ActivateMissionSuccededMenu;
    }

    // Update is called once per frame
    void Update()
    {
        float rounded = Mathf.Round(TimeController.instance._currentPassedTime * 10) / 10;
        remainingTime.text = rounded.ToString();
        remainingCollect.text = GameManager.instance.currentCollectable + "/" + GameManager.instance.maxCollectable;
        
    }

    public void GetCollectedItemHud() //Sets on GameManager
    {
        for (int i = 0; i < CollectingItems.childCount; i++)
        {
            Debug.Log(CollectingItems.GetChild(i).gameObject.name);
            if (GameManager.instance.collectName == CollectingItems.GetChild(i).gameObject.name)
            {
                Debug.Log("Seçilen Obje: " + CollectingItems.GetChild(i).gameObject.name);
                CollectingItems.GetChild(i).gameObject.SetActive(true);

            }
        }
    }

    public void ActivateStartGameButton() // Calls on Object Spawner
    {
        startGameButton.SetActive(true);
    }
    private void ActivateCollectHud()
    {
        CollectingItems.gameObject.SetActive(true);
        collectImage.SetActive(true);
    }
    private void DeactivateCollectHud()
    {
        CollectingItems.gameObject.SetActive(false);
        collectImage.SetActive(false);
    }
    private void ActivateMissionSuccededMenu()
    {
        missionSucceded.SetActive(true);
    }

    private void ActivateMissionFailedMenu()
    {
        missionFailed.SetActive(true);
    }

    public void OnStartGameClicked()
    {
        GameManager.instance.StartGame();
    }

    public void OnHomeButtonPressed()
    {
        GameManager.instance.GoHomeMenu();
    }
    
}

