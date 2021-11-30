using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LionStudios.Suite.Analytics;
using LionStudios.Suite.Debugging;
using com.adjust.sdk;
using GameAnalyticsSDK;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager instance;

    public event Action onStartGame;
    public event Action onGameOver;
    public event Action onMissionCompleted;
    public event Action onBombExploaded;


    //[SerializeField] private bool startGame;
    [SerializeField] private string _collectName;
    public ParticleSystem _particle;
    public ParticleSystem _konfeti;
    public ParticleSystem _fireworks;

    [SerializeField] private ProgressBar collectedProgressBar;



    public string collectName
    {
        get
        {
            return _collectName;
        }
        set
        {
            _collectName = value;
            UIManager.instance.GetCollectedItemHud();
        }
    }

    public int maxCollectable;
    [SerializeField] private int _currentCollactable;

    private bool isTimeSlowed;
    public int currentCollectable
    {
        get
        {
            return _currentCollactable;
        }
        set
        {
            _currentCollactable = value;
            collectedProgressBar.FillAmount(maxCollectable, _currentCollactable);
            MissionCompleted();
        }
    }

    [SerializeField] private Transform sceneObjects;
    private Rigidbody[] objects;

    #endregion

    #region Singleton and SDK Initializiton
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        //startGame = true;
    }

    
    void Start()
    {
        GameAnalytics.Initialize();
        LionDebugger.Hide();
        LionAnalytics.LevelStart(SceneManager.GetActiveScene().buildIndex, 1);
            


#if UNITY_IOS
        /* Mandatory - set your iOS app token here */
        InitAdjust("YOUR_IOS_APP_TOKEN_HERE");
#elif UNITY_ANDROID
            /* Mandatory - set your Android app token here */
            InitAdjust("ew6ppwvmr5z4");
        
#endif
            objects = new Rigidbody[sceneObjects.childCount];
        for (int i = 0; i < sceneObjects.childCount; i++)
        {
            objects[i] = sceneObjects.GetChild(i).GetComponent<Rigidbody>();
        }
        
    }

    private void InitAdjust(string adjustAppToken)
    {
        var adjustConfig = new AdjustConfig(
            adjustAppToken,
            AdjustEnvironment.Production, // AdjustEnvironment.Sandbox to test in dashboard
            true
        );
        adjustConfig.setLogLevel(AdjustLogLevel.Info); // AdjustLogLevel.Suppress to disable logs
        adjustConfig.setSendInBackground(true);
        new GameObject("Adjust").AddComponent<Adjust>(); // do not remove or rename

        // Adjust.addSessionCallbackParameter("foo", "bar"); // if requested to set session-level parameters

        //adjustConfig.setAttributionChangedDelegate((adjustAttribution) => {
        //  Debug.LogFormat("Adjust Attribution Callback: ", adjustAttribution.trackerName);
        //});

        Adjust.start(adjustConfig);

    }

    #endregion

    #region StartEvents
    public void StartGame()  //Calls on object spawner
    {
        onStartGame?.Invoke();
    }
    public void BombExploaded()
    {
        onBombExploaded?.Invoke();
    }

    

    public void GameOver()
    {
        onGameOver?.Invoke();
        MakeTimeGoForward();
        Debug.Log("Game OVer!!");
        SoundManager.instance.PlayMusic(Soundlar.Lose);
        LionAnalytics.LevelFail(SceneManager.GetActiveScene().buildIndex, 1);
    }


    public void MissionCompleted()
    {
        if (currentCollectable >= maxCollectable)
        {
            onMissionCompleted?.Invoke();
            CheckIfCurrentLevelIncreased();
            MakeTimeGoForward();

            Debug.Log("MissionCompleted");
            SoundManager.instance.PlayMusic(Soundlar.Win);


            Instantiate(_konfeti, Vector3.up * 20f, Quaternion.identity);
            Instantiate(_fireworks, Vector3.up * 7f, Quaternion.identity);
            Instantiate(_fireworks, Vector3.up * 3.5f + Vector3.forward * 3.5f, Quaternion.identity);
            Instantiate(_fireworks, Vector3.up * 3.5f + Vector3.right * 3.5f, Quaternion.identity);


            LionAnalytics.LevelComplete(SceneManager.GetActiveScene().buildIndex, 1);
        }
    }

    #endregion


    #region TimeEffects
    public void SlowDownTime() //Calls on Bomb when Ignite
    {
        if (!isTimeSlowed)
        {
            TimeController.instance.TimeEffectsOnStartingGame();
            isTimeSlowed = true;
        }
    }


    public void MakeTimeGoForward()
    {
        TimeController.instance.SlowTime(true);
    }
    #endregion

    #region GameOver and Level Loading
    public void CheckIfGameOver(float currentTime) // Checks in TimeController
    {
        if (currentTime <= 0)
            GameOver();
    }

    public void LoadLevelAgain()
    {
        LionAnalytics.LevelRestart(SceneManager.GetActiveScene().buildIndex, 1);
        int a = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(a);
    }
    public void NextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void GoHomeMenu()
    {
        SceneManager.LoadScene("menu");
    }

    public void LoadEndless()
    {
        SceneManager.LoadScene("kitchen");
    }
    private void CheckIfCurrentLevelIncreased()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex - 1;
        if(currentLevel == LevelManager.activeLevel)
        {
            LevelManager.activeLevel += 1;
            SaveManager.instance.Save();
        }
    }

    #endregion
}
