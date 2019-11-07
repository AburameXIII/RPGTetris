using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Animator Animator;
    public void Clear()
    {
        Animator.SetTrigger("Clear");
        BGMusic.Instance.PlayClear();

    }

    public void Erase()
    {
        Destroy(gameObject);
    }
}
