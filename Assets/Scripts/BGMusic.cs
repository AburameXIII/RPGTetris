using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    private static BGMusic instance = null;
    public static BGMusic Instance
    {
        get { return instance;  }
    }

    public AudioSource Music;
    public AudioSource PieceLand;
    public AudioSource Clear;
    public AudioSource Fireball;
    public AudioSource Gravity;
    public AudioSource Polimorph;

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
    }

    public void DecreasePitch(float value)
    {
        Music.pitch -= 1 / value;
    }

    public void ResetPitch()
    {
        Music.pitch = 1;
    }

    public void PlayPolimorph()
    {
        Polimorph.Play();
    }

    public void PlayGravity()
    {
        Gravity.Play();
    }

    public void PlayFireball()
    {
        Fireball.Play();
    }

    public void PlayClear()
    {
        Clear.Play();
    }

    public void PlayPieceLand()
    {
        PieceLand.Play();
    }
}
