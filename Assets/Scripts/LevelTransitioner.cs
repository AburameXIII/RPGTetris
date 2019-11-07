using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitioner : MonoBehaviour
{
    int Scene;
    public Animator Fader;

    public void TransitionToLevel(int i)
    {
        Fader.SetTrigger("FadeOut");
        Scene = i;
    }

    public void SimpleTransitionToLevel(int i)
    {
        Fader.SetTrigger("SimpleFade");
        Scene = i;
    }

    public void GameOver()
    {
        Fader.SetTrigger("SimpleFade");
        Scene = 9;
    }

    public void OnFadeOut()
    {
        SceneManager.LoadScene(Scene);
    }
}
