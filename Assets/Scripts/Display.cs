using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    public Animator PieceAnimator;
    public int DisplayPieceNumber = -1;
    
    public void ChangeDisplayPiece(int i)
    {
        DisplayPieceNumber = i;
        PieceAnimator.SetInteger("Piece", i);
    }
}
