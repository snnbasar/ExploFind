using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using LionStudios.Suite.Analytics;
using LionStudios.Suite.Debugging;
using com.adjust.sdk;
public enum Menu
{
    Natural,
    PlayMenu,
    GameModeMenu,
    Levels1,
    Levels2
}

public class MenuScene : MonoBehaviour
{
    [SerializeField] private Transform objects; //sets on inspector
    [SerializeField] private float timeScale = 0.01f;

    [SerializeField] private Transform spawnPoint1; //sets on inspector
    [SerializeField] private Transform spawnPoint2; //sets on inspector

    private float fixedDeltaTime;


    public GameObject playMenu;
    public GameObject gameMode;
    public GameObject levels;
    public GameObject levels1;
    public GameObject levels2;
    void Start()
    {
        OrganizeObjectsAndTime();
        LionDebugger.Hide();
        LionAnalytics.GameStart();
#if UNITY_IOS
        /* Mandatory - set your iOS app token here */
        InitAdjust("YOUR_IOS_APP_TOKEN_HERE");
#elif UNITY_ANDROID
        /* Mandatory - set your Android app token here */
        InitAdjust("ew6ppwvmr5z4");
#endif
    }


    public void ChangeMenu(int index)
    {
        switch ((Menu)index)
        {
            case Menu.Natural:
                playMenu.SetActive(false);
                gameMode.SetActive(false);
                levels.SetActive(false);
                levels1.SetActive(false);
                levels2.SetActive(false);
                break;
            case Menu.PlayMenu:
                playMenu.SetActive(true);
                break;
            case Menu.GameModeMenu:
                gameMode.SetActive(true);
                break;
            case Menu.Levels1:
                levels.SetActive(true);
                levels1.SetActive(true);
                break;
            case Menu.Levels2:
                levels.SetActive(true);
                levels2.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void LoadEndless()
    {
        SceneManager.LoadScene("kitchen");
    }


    #region SceneObjects and Time
    private void OrganizeObjectsAndTime()
    {
        this.fixedDeltaTime = 0.02f;
        Time.fixedDeltaTime = 0.02f;
        Time.timeScale = 1;
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        for (int i = 0; i < objects.childCount; i++)
        {
            objects.GetChild(i).localScale *= 3.7f;
            objects.GetChild(i).position = selectRandomPoint();
            objects.GetChild(i).rotation = Random.rotation;
        }
    }
    private Vector3 selectRandomPoint()
    {
        float x = Random.Range(spawnPoint1.position.x, spawnPoint2.position.x);
        float y = Random.Range(spawnPoint1.position.y, spawnPoint2.position.y);
        float z = Random.Range(spawnPoint1.position.z, spawnPoint2.position.z);
        return new Vector3(x, y, z);
    }

    #endregion


    public void newPlayGame()
    {
        if (LevelManager.activeLevel < LevelManager.maxLevel)
        {
            int a = LevelManager.activeLevel + 1;
            SceneManager.LoadScene(a);
        }
        else
        {
            SceneManager.LoadScene("kitchen");
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
}
