using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPiece : MonoBehaviour
{
    Animator PieceAnimator;
    public int NextPieceNumber = -1;

    // Start is called before the first frame update
    void Start()
    {
        PieceAnimator = GetComponent<Animator>();
    }

    public void NextPieceChange(int i)
    {
        NextPieceNumber = i;
        PieceAnimator.SetInteger("Piece", NextPieceNumber);
    }
}
