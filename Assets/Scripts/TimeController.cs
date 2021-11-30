using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    #region Variables
    public static TimeController instance;


    [SerializeField] private AnimationCurve animationCurveSlow;
    [SerializeField] private AnimationCurve animationCurveFast;
    [SerializeField] private float timeMultiplayer = 0.5f;

    [SerializeField] private float timeMinSlowDown = 0.01f;

    public int timeToPassLevel = 20;
    [SerializeField] private float currentPassedTime;


    private bool stopTimer;

    public float _currentPassedTime
    {
        get
        {
            return currentPassedTime;
        }
        set
        {
            if (currentPassedTime < 0)
                currentPassedTime = 0;
            else
                currentPassedTime = value;
            
            //GameManager.instance.CheckIfGameOver(currentPassedTime);
        }
    }

    public float testScale;

    static float t = 0.0f;

    private float fixedDeltaTime;
    #endregion

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
        this.fixedDeltaTime = 0.02f;
        Time.fixedDeltaTime = 0.02f;
        Time.timeScale = 1;
        GameManager.instance.onGameOver += StopCurrentTime;
        GameManager.instance.onMissionCompleted += StopCurrentTime;
        _currentPassedTime = timeToPassLevel;
    }

    public void TimeEffectsOnStartingGame()
    {
        StartLevelPassTime();

        SlowTime(false);
        
    }
    public async void SlowTime(bool forward) //if true set the time go forward else go backwards
    {
        float min = 1f;
        float max = timeMinSlowDown;
        t = 0;
        AnimationCurve animCurve = animationCurveSlow;
        if (forward)
        {
            min = timeMinSlowDown;
            max = 1f;
            animCurve = animationCurveFast;
            timeMultiplayer *= 2;
        }
        while (t <= 1f)
        {
            testScale = Time.timeScale;
            Time.timeScale = Mathf.Lerp(min, max, animCurve.Evaluate(t));
            t += timeMultiplayer * Time.deltaTime;
            
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            await Task.Yield();
        }
    }



    public void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }


    public async void StartLevelPassTime() // Fixed Timer for game to stop while time scale is changing
    {
        float fixedToplam = 0;
        bool sifirla = true;

        while (_currentPassedTime >= 0) 
        {
            if (stopTimer)
            {
                sifirla = false;
                break;
            }
            fixedToplam += Time.deltaTime / Time.timeScale;
            _currentPassedTime = (timeToPassLevel - fixedToplam) ;
            await Task.Yield();
        }
        if(sifirla)
            _currentPassedTime = 0;
    }

    private void StopCurrentTime()
    {
        stopTimer = true;
    }

}
