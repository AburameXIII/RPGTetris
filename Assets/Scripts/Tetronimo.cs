using UnityEngine;

[CreateAssetMenu(fileName ="New Tetromino", menuName ="Piece")]
public class Tetronimo : ScriptableObject
{
    public int PieceNumber;
    public GameObject PiecePrefab;
}