using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    Animator PieceAnimator;

    // Start is called before the first frame update
    void Start()
    {
        PieceAnimator = GetComponent<Animator>();
    }

    public void ChangeStoragePiece(int i)
    {
        PieceAnimator.SetInteger("Piece", i);
    }
}
