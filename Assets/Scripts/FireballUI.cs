using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireballUI : MonoBehaviour
{
    public Text BlocksDestroyed;
    Vector2 ObjectivePosition;
    Vector3 InitialPosition;
    float TimeStart;
    public float OffSetY;
    public float OffSetX;

    public Animator Animator;


    private static FireballUI instance;
    public static FireballUI Instance
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
    }

    public void ChangeDisplay(int i)
    {
        BlocksDestroyed.text = i.ToString();
    }


    public void SetObjectivePosition(Vector3 position)
    {
        ObjectivePosition = Camera.main.WorldToScreenPoint(position);
        ObjectivePosition.x += OffSetX;
        ObjectivePosition.x += OffSetY;
        InitialPosition = transform.position;
        TimeStart = Time.time;
    }


    private void Update()
    {
        float t = ((Time.time - TimeStart) / GameManager.CurrentFallInterval);

        
        transform.position = new Vector3 (Mathf.Lerp(InitialPosition.x, ObjectivePosition.x, t), Mathf.Lerp(InitialPosition.y, ObjectivePosition.y, t),0);
    }

    public void Appear()
    {
        Animator.SetTrigger("Appear");
    }

    public void Disappear()
    {
        Animator.SetTrigger("Disappear");
    }

}
