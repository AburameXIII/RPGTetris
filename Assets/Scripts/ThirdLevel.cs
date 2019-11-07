using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdLevel : MonoBehaviour
{
    public GameObject Brick;
    // Start is called before the first frame update
    void Start()
    {
        Playfield.insertLines(8, Brick);
    }

}
