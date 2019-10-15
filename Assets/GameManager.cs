using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public List<GameObject> Pieces;
    public List<GameObject> NextPieces;
    public List<NextPiece> NextPiecesUI;


    public static float LastFall = 0;

    public Storage Store;
    bool StorageLocked = false;
    int StoredPieceNumber = -1;

    public static int PieceNumber = 0;
    public static GameObject fallingPiece;

    public GameObject chooseRandomPiece()
    {
        int i = Random.Range(0, Pieces.Count);
        NextPiecesUI[0].NextPieceChange(NextPiecesUI[1].NextPieceNumber);
        NextPiecesUI[1].NextPieceChange(NextPiecesUI[2].NextPieceNumber);
        NextPiecesUI[2].NextPieceChange(NextPiecesUI[3].NextPieceNumber);
        NextPiecesUI[3].NextPieceChange(i);
        return Pieces[i];
    }


    public void spawnNext()
    {
        if(Playfield.IsBlockAbovePlayZone())
        {
            Debug.Log("Game Over");
        } else
        {
            fallingPiece = Instantiate(NextPieces[0], transform.position, Quaternion.identity);
            PieceNumber = int.Parse(NextPieces[0].tag);
            NextPieces.RemoveAt(0);
            NextPieces.Add(chooseRandomPiece());
            StorageLocked = false;
        }
    }

    public void SpawnPieceX(int i)
    {
        PieceNumber = i;
        fallingPiece = Instantiate(Pieces[i], transform.position, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            NextPieces.Add(chooseRandomPiece());
        }
        spawnNext();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q) && !StorageLocked)
        {
            int PieceToStore = GameManager.PieceNumber;
            Destroy(GameManager.fallingPiece);



            if (StoredPieceNumber >= 0)
            {
                SpawnPieceX(StoredPieceNumber);
            }
            else
            {
                FindObjectOfType<GameManager>().spawnNext();
            }


            StoredPieceNumber = PieceToStore;
            Store.ChangeStoragePiece(StoredPieceNumber);
            StorageLocked = true;
        }
    }
}
