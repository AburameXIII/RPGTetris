using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    int number;
    public Vector3 RotationPoint;
    // Start is called before the first frame update
    void Start()
    {
        if(!isValidFieldPosition())
        {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;

            if (!isValidFieldPosition())
            {
                transform.position += Vector3.right;
            }
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;

            if (!isValidFieldPosition())
            {
                transform.position += Vector3.left;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), Vector3.forward, 90);

            if (!isValidFieldPosition())
            {
                transform.RotateAround(transform.TransformPoint(RotationPoint), Vector3.forward, -90);
            }
        }

        if (Input.GetKey(KeyCode.DownArrow) || Time.time - GameManager.LastFall >= 1)
        {
            transform.position += Vector3.down;

            if (!isValidFieldPosition())
            {
                transform.position += Vector3.up;

                AddToField();

                Playfield.deleteFullRows();

                FindObjectOfType<GameManager>().spawnNext();

                enabled = false;
            }

            GameManager.LastFall = Time.time;
        }
    }

    bool isValidFieldPosition()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);
            if (!Playfield.insideBorder(new Vector2(x,y)))
                return false;

            if (Playfield.Field[x,y] != null &&
                Playfield.Field[x,y].parent != transform)
                return false;
        }
        return true;
    }

    void AddToField()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);
            
            Playfield.Field[x,y] = child;
        }
    }
}
