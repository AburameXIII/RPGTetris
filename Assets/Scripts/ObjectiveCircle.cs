using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveCircle : MonoBehaviour
{
    public Image Circle;
    public Text Progress;
    public Text Objective;
    private float ObjectiveLines;
    private float fraction;
    public float Duration;
    private float TimeStart;
    private float InitialAmount;

    public void UpdateCircle()
    {
        fraction = (float)GameManager.CurrentLines / ObjectiveLines;
        if (fraction < 0 || fraction > 1)
            fraction = fraction < 0 ? 0 : 1;

        Progress.text = GameManager.CurrentLines.ToString();
        TimeStart = Time.time;
        InitialAmount = Circle.fillAmount;
    }


    private void Update()
    {
        float t = ((Time.time - TimeStart) / Duration);
        Circle.fillAmount = Mathf.Lerp(InitialAmount, fraction, t);
    }

    public void SetObjective(int Lines)
    {
        ObjectiveLines = Lines;
        Objective.text = ObjectiveLines.ToString();
    }

    public void IncrementProgress()
    {
        GameManager.CurrentLines += 1;
        UpdateCircle();
    }
}
