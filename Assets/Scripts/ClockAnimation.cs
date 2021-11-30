using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockAnimation : MonoBehaviour
{
    [SerializeField] private Transform clockSecond;

    [SerializeField] private float time;
    private int maxTime;
    [SerializeField] private float timeMultiplayer = 12;
    private bool start;
    [SerializeField] private bool ui;

    // Start is called before the first frame update
    void Start()
    {
        //GameManager.instance.onBombExploaded += StartClocks;
        maxTime = TimeController.instance.timeToPassLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            time = maxTime - TimeController.instance._currentPassedTime;
            if (time <= 60 && ui)
                clockSecond.eulerAngles = new Vector3(0, 0, -1 * time * timeMultiplayer);
            if (time <= 60 && !ui)
                clockSecond.eulerAngles = new Vector3(0, 180, 1 * time * timeMultiplayer);
        }
    }

    private void StartClocks()
    {
        start = true;
    }
}
