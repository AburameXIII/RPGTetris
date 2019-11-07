using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    public float TimeStart;
    public Image Clock;
    bool DecreaseClock;

    private static ClockUI instance;
    public static ClockUI Instance
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

        Clock.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (DecreaseClock)
        {
            float t = ((Time.time - TimeStart) / 10);
            Clock.fillAmount = Mathf.Lerp(1, 0, t);

            if (Clock.fillAmount == 0) DecreaseClock = false;
        }
        
    }


    public void ClockStart()
    {
        DecreaseClock = true;
        TimeStart = Time.time;
        Clock.fillAmount = 1;
    }

}
