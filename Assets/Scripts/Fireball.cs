using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Piece
{
    public bool SpawnNextOnce = false;
    public bool Autopilot = false;
    public bool Outside = true;
    float AutopilotLastFall;
    public int DestroyedBlocks = 0;

    protected override void Start()
    {
        FireballUI.Instance.transform.position=Camera.main.WorldToScreenPoint(transform.position);
        FireballUI.Instance.SetObjectivePosition(transform.position);
        FireballUI.Instance.ChangeDisplay(DestroyedBlocks);
        FireballUI.Instance.Appear();
    }

    protected override void MoveDown()
    {
        if (Autopilot)
        {
            if (Time.time - AutopilotLastFall >= GameManager.CurrentFallInterval / 2)
            {
                transform.position += Vector3.down;
                AutopilotLastFall = Time.time;
            }

            if (!SpawnNextOnce)
            {
                SpawnNextOnce = true;
                FindObjectOfType<GameManager>().SpawnNext();
                FireballUI.Instance.Disappear();
            }
        }
        else
        {
            if ((Time.time - GameManager.LastFall >= (Input.GetKey(KeyCode.DownArrow) ? GameManager.CurrentFallInterval / GameManager.FallSpeedUp / 2 : GameManager.CurrentFallInterval / 2)))
            {
                transform.position += Vector3.down;

                FireballUI.Instance.SetObjectivePosition(transform.position);
                DestroyPieces();

                //If Completly outside
                if (Outside)
                {
                    Autopilot = true;
                }
                else
                {
                    Outside = true;
                }

                GameManager.LastFall = Time.time;
            }


        }

    }

    void DestroyPieces()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);
            if (Playfield.insideBorder(new Vector2(x, y)))
            {
                if (Playfield.Field[x, y] != null)
                {
                    Playfield.deletePosition(x, y);


                    DestroyedBlocks++;
                    FireballUI.Instance.ChangeDisplay(DestroyedBlocks);
                    if (DestroyedBlocks % 10 == 0)
                    {
                        FindObjectOfType<GameManager>().IncrementCurrentLines();
                    }
                }

                Outside &= false;
            }
            else
            {
                Outside &= true;
            }

        }
    }

    protected override void MoveLeft()
    {
        if (!Autopilot)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left;

                if (!IsInsideBorder())
                {
                    transform.position += Vector3.right;
                }
                else
                {
                    DestroyPieces();
                    FireballUI.Instance.SetObjectivePosition(transform.position);
                }
            }
        }
    }


    bool IsInsideBorder()
    {
        foreach (Transform child in transform)
        {
            int x = Mathf.RoundToInt(child.position.x);
            int y = Mathf.RoundToInt(child.position.y);
            if (!Playfield.insideVerticalBorder(new Vector2(x, y)))
            {
                return false;
            }
        }
        return true;
    }

    protected override void MoveRight()
    {
        if (!Autopilot)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += Vector3.right;

                if (!IsInsideBorder())
                {
                    transform.position += Vector3.left;
                }
                else
                {
                    DestroyPieces();
                    FireballUI.Instance.SetObjectivePosition(transform.position);
                }
            }
        }
    }

    protected override void Rotate()
    { }

    public override void InstantFall()
    {
    }
}
