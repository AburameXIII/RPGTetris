using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{

    public int NextLevelScene;
    public LevelTransitioner Transitioner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            Transitioner.TransitionToLevel(NextLevelScene);
        }
    }
}
