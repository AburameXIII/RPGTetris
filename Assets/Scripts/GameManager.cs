using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public List<Tetronimo> Pieces; //List of all pieces, needs to make sure each piece number corresponds to its index
    public List<Tetronimo> NextPieces; //Next 4 pieces to spawn, this list has always 4 members


    public List<Display> NextPiecesUI; //UI of the next pieces display
    public ObjectiveCircle Progress; //UI to measure the player progress
    public Display Storage; //UI of the piece currently stored


    public static float LastFall = 0; //To count the piece falling Timing 
    public float FallInterval = 0.25f; //Falling speed of the current level (value never changes during the level)
    public static float CurrentFallInterval; //Based on the fall time, used for time skills (value can change during the level)
    public static float FallSpeedUp = 10; //Fall speed up when player clicks the down key 

    public int ObjectiveLines;
    public static int CurrentLines = 0;


    bool StorageLocked = false; //Makes sure the player can only store a piece at one given time
    int StoredPieceNumber = -1; //The number of the Piece currently stored
    int PieceNumber = 0; //Number of the current piece falling
    GameObject FallingPiece; //Current piece falling

    float SlowTimeLastUsed = -Mathf.Infinity;
    public float SlowTimeDuration;
    public float SlowTimeCooldown;
    public Image SlowTimeCircle;

    float FireballLastUsed = -Mathf.Infinity;
    public float FireballCooldown;
    public Image FireballCircle;

    float PolimorphLastUsed = -Mathf.Infinity;
    public float PolimorphCooldown;
    public Image PolimorphCircle;

    float GravityLastUsed = -Mathf.Infinity;
    public float GravityCooldown;
    public Image GravityCircle;

    public LevelTransitioner Transitioner;
    public GameObject Brick;
    public int InitialLines;

    public int NextLevelScene;
    public enum Level
    {
        Level1,
        Level2,
        Level3
    }

    public Level CurrentLevel;
    public Animator MageAnimator;


    public void IncrementCurrentLines()
    {
        CurrentLines += 1;
        Progress.UpdateCircle();
    }


    public Tetronimo ChooseRandomPiece()
    {
        //Chooses from the default tetronimos
        int i = Random.Range(0, 7);

        //returns the new Object top spawn
        return Pieces[i];
    }


    public void SpawnNext()
    {
        //Game Over if can't spawn piece
        if (Playfield.IsBlockAbovePlayZone())
        {
            Playfield.deleteAll();
            Timer.Instance.Stop();
            Transitioner.GameOver();
            Debug.Log("Game Over - Game Manager");
        }
        else
        {
            //Instantiates the Next Piece to spawn
            FallingPiece = Instantiate(NextPieces[0].PiecePrefab,
                transform.position,
                Quaternion.identity);
            PieceNumber = NextPieces[0].PieceNumber;
            NextPieces.RemoveAt(0);
            NextPieces.Add(ChooseRandomPiece());

            NextPiecesUI[0].ChangeDisplayPiece(NextPieces[0].PieceNumber);
            NextPiecesUI[1].ChangeDisplayPiece(NextPieces[1].PieceNumber);
            NextPiecesUI[2].ChangeDisplayPiece(NextPieces[2].PieceNumber);
            NextPiecesUI[3].ChangeDisplayPiece(NextPieces[3].PieceNumber);

            StorageLocked = false;
        }
    }

    public void SpawnPiece(int i)
    {
        PieceNumber = i;
        FallingPiece = Instantiate(Pieces[i].PiecePrefab, transform.position, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        LastFall = 0;
        CurrentLines = 0;
        CurrentFallInterval = FallInterval;
        Playfield.deleteAll();
        Playfield.insertLines(InitialLines, Brick);

        //Chooses 4 initial random pieces
        Tetronimo InitialPiece0 = ChooseRandomPiece();
        Tetronimo InitialPiece1 = ChooseRandomPiece();
        Tetronimo InitialPiece2 = ChooseRandomPiece();
        Tetronimo InitialPiece3 = ChooseRandomPiece();

        //Adds to the list of Next Pieces to spawn
        NextPieces = new List<Tetronimo>();
        NextPieces.Add(InitialPiece0);
        NextPieces.Add(InitialPiece1);
        NextPieces.Add(InitialPiece2);
        NextPieces.Add(InitialPiece3);

        //Updates de display of each UI
        NextPiecesUI[0].ChangeDisplayPiece(InitialPiece0.PieceNumber);
        NextPiecesUI[0].ChangeDisplayPiece(InitialPiece0.PieceNumber);
        NextPiecesUI[0].ChangeDisplayPiece(InitialPiece0.PieceNumber);
        NextPiecesUI[0].ChangeDisplayPiece(InitialPiece0.PieceNumber);

        //Spawns the first Piece
        SpawnNext();

        Progress.SetObjective(ObjectiveLines);
        BGMusic.Instance.ResetPitch();

        if(CurrentLevel == Level.Level1)
        {
            Timer.Instance.StartTimer();
        } else
        {
            Timer.Instance.Resume();
        }
    }

    // Update is called once per frame
    void Update()
    {

        Store();
        Polimorph();
        Fireball();
        Gravity();
        SlowTime();

        CheckObjective();
    }

    void CheckObjective()
    {
        if (CurrentLines >= ObjectiveLines)
        {
            if (CurrentLevel == Level.Level3)
            {
                Timer.Instance.Stop();
                Transitioner.SimpleTransitionToLevel(NextLevelScene);
            }
            else
            {

                Timer.Instance.Pause();
                Transitioner.TransitionToLevel(NextLevelScene);
            }

            Playfield.deleteAll();

        }
    }

    public void Store()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !StorageLocked)
        {
            int PieceToStore = PieceNumber;
            Destroy(FallingPiece);



            if (StoredPieceNumber >= 0)
            {
                SpawnPiece(StoredPieceNumber);
            }
            else
            {
                FindObjectOfType<GameManager>().SpawnNext();
            }


            StoredPieceNumber = PieceToStore;
            Storage.ChangeDisplayPiece(StoredPieceNumber);
            StorageLocked = true;
        }
    }


    public void Fireball()
    {
        //Check Cooldown
        if (Time.time - FireballLastUsed < FireballCooldown)
        {
            float t = ((Time.time - FireballLastUsed) / FireballCooldown);
            FireballCircle.fillAmount = Mathf.Lerp(1, 0, t);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Perform Skill
            MageAnimator.SetTrigger("Cast");
            BGMusic.Instance.PlayFireball();
            FireballLastUsed = Time.time;
            NextPieces[0] = Pieces[8];
            NextPiecesUI[0].ChangeDisplayPiece(Pieces[8].PieceNumber);
        }
    }

    public void Polimorph()
    {
        //Check Cooldown
        if (Time.time - PolimorphLastUsed < PolimorphCooldown)
        {
            float t = ((Time.time - PolimorphLastUsed) / PolimorphCooldown);
            PolimorphCircle.fillAmount = Mathf.Lerp(1, 0, t);
            return;
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Perform Skill
            MageAnimator.SetTrigger("Cast");
            BGMusic.Instance.PlayPolimorph();
            PolimorphLastUsed = Time.time;
            NextPieces[0] = Pieces[7];
            NextPiecesUI[0].ChangeDisplayPiece(Pieces[7].PieceNumber);
        }
    }

    public void Gravity()
    {
        //Check Cooldown
        if (Time.time - GravityLastUsed < GravityCooldown)
        {
            float t = ((Time.time - GravityLastUsed) / GravityCooldown);
            GravityCircle.fillAmount = Mathf.Lerp(1, 0, t);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && CurrentLevel == Level.Level3)
        {
            //Perform Skill
            MageAnimator.SetTrigger("Cast");
            BGMusic.Instance.PlayGravity();
            GravityLastUsed = Time.time;
            FallingPiece.GetComponent<Piece>().InstantFall();
            Playfield.decreaseAllColumns();
            Playfield.deleteFullRows();
        }
    }


    public void SlowTime()
    {
        //Check Cooldown
        if (Time.time - SlowTimeLastUsed < SlowTimeCooldown)
        {
            float t = ((Time.time - SlowTimeLastUsed) / SlowTimeCooldown);
            SlowTimeCircle.fillAmount = Mathf.Lerp(1, 0, t);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && (CurrentLevel == Level.Level3 || CurrentLevel == Level.Level2))
        {
            //Perform Skill
            MageAnimator.SetTrigger("Cast");
            SlowTimeLastUsed = Time.time;
            StartCoroutine(ChangeFallSpeed());
        }
    }

    IEnumerator ChangeFallSpeed()
    {

        CurrentFallInterval = FallInterval * 4;
        BGMusic.Instance.DecreasePitch(4);
        ClockUI.Instance.ClockStart();
        yield return new WaitForSeconds(SlowTimeDuration);
        CurrentFallInterval = FallInterval;
        BGMusic.Instance.ResetPitch();
    }
}
