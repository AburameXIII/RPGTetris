using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    private static Timer instance = null;
    public static Timer Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        BestTime=PlayerPrefs.GetFloat("BestTime",Mathf.Infinity);
    }

    public Text TimerText;
    private float StartTime;
    private float TimeAccumulated = 0;
    public float TimeTotal;
    private bool Paused = true;
    public Animator Animator;
    public float BestTime;


    // Update is called once per frame
    void Update()
    {
        if (!Paused)
        {
            TimeTotal = Time.time - StartTime + TimeAccumulated;

            string minutes = ((int)TimeTotal / 60).ToString();
            string seconds = (TimeTotal % 60).ToString("f2");

            if (TimeTotal % 60 < 10) seconds = "0" + seconds;
            if ((int)TimeTotal / 60 < 10) minutes = "0" + minutes;

            TimerText.text = minutes + ":" + seconds;
        }
    }

    public void StartTimer()
    {
        StartTime = Time.time;
        Animator.ResetTrigger("Stop");
        Animator.SetTrigger("StartTimer");
        Paused = false;
    }

    public void RestartTimer()
    {
        TimeAccumulated = 0;
    }

    public void Pause()
    {
        TimeAccumulated = TimeTotal;
        Paused = true;
    }

    public void Resume()
    {
        StartTime = Time.time;
        Paused = false;
    }

    public void Stop()
    {
        Pause();
        Animator.SetTrigger("Stop");
        Animator.ResetTrigger("StartTimer");
        RestartTimer();
    }

    public void UpdateScore()
    {
        if(TimeTotal < BestTime)
        {
            BestTime = TimeTotal;
            PlayerPrefs.SetFloat("BestTime", BestTime);
            PlayerPrefs.Save();
        }
    }
}
