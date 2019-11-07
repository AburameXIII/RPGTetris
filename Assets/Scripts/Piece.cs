using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    int number;
    public Vector3 RotationPoint;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(!isValidFieldPosition())
        {
            Debug.Log("GAME OVER - PIECE");
            Destroy(gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveLeft();
        MoveRight();
        Rotate();
        MoveDown();
        InstantFall();
    }

    protected virtual void MoveLeft()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left;

            if (!isValidFieldPosition())
            {
                transform.position += Vector3.right;
            }
        }
    }

    protected virtual void MoveRight()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right;

            if (!isValidFieldPosition())
            {
                transform.position += Vector3.left;
            }
        }
    }

    protected virtual void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(RotationPoint), Vector3.forward, -90);

            if (!isValidFieldPosition())
            {
                transform.RotateAround(transform.TransformPoint(RotationPoint), Vector3.forward, 90);
            }
        }
    }

    protected virtual void MoveDown()
    {
        if (Time.time - GameManager.LastFall >= (Input.GetKey(KeyCode.DownArrow) ? GameManager.CurrentFallInterval / GameManager.FallSpeedUp : GameManager.CurrentFallInterval))
        {
            transform.position += Vector3.down;

            if (!isValidFieldPosition())
            {
                transform.position += Vector3.up;

                AddToField();
                BGMusic.Instance.PlayPieceLand();


                Playfield.deleteFullRows();

                FindObjectOfType<GameManager>().SpawnNext();

                enabled = false;
            }

            GameManager.LastFall = Time.time;
        }
    }


    public virtual void InstantFall()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            while (isValidFieldPosition())
            {
                transform.position += Vector3.down;
            }

            transform.position += Vector3.up;

            AddToField();
            BGMusic.Instance.PlayPieceLand();

            Playfield.deleteFullRows();

            FindObjectOfType<GameManager>().SpawnNext();

            enabled = false;
        }
    }


    protected bool isValidFieldPosition()
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
