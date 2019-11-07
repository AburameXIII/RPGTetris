using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clear : MonoBehaviour
{
    public Text HighScore;
    public Text CurrentScore;
    public Text Record;
    public int NextLevelScene;
    public LevelTransitioner Transitioner;
    // Start is called before the first frame update
    void Start()
    {
        Timer.Instance.UpdateScore();
        string minutesBT = ((int)Timer.Instance.BestTime / 60).ToString();
        string secondsBT = (Timer.Instance.BestTime % 60).ToString("f2");

        if (Timer.Instance.BestTime % 60 < 10) secondsBT = "0" + secondsBT;
        if ((int)Timer.Instance.BestTime / 60 < 10) minutesBT = "0" + minutesBT;

        string minutesTT = ((int)Timer.Instance.TimeTotal / 60).ToString();
        string secondsTT = (Timer.Instance.TimeTotal % 60).ToString("f2");

        if (Timer.Instance.TimeTotal % 60 < 10) secondsTT = "0" + secondsTT;
        if ((int)Timer.Instance.TimeTotal / 60 < 10) minutesTT = "0" + minutesTT;

        HighScore.text = "High Score: " + minutesBT + ":" + secondsBT;
        CurrentScore.text = "Current Score: " + minutesTT + ":" + secondsTT;

        if(Timer.Instance.BestTime == Timer.Instance.TimeTotal)
        {
            Record.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Transitioner.TransitionToLevel(NextLevelScene);
        }
    }
}
