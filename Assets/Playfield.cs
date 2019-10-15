using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    // Start is called before the first frame update


    public static int w = 10;
    public static int h = 24;
    public static Transform[,] Field = new Transform[w, h];


    public static bool insideBorder(Vector2 position)
    {
        return (int)position.x >= 0 && (int)position.x < w && (int)position.y >= 0 && (int)position.y < h;
    }
    


    public static void deleteRow(int row)
    {
        for(int x = 0; x < w; x++)
        {
            Destroy(Field[x, row].gameObject);
            Field[x, row] = null;
        }
    }

    public static void decreaseRow(int row)
    {
        for (int x = 0; x < w; x++)
        {
            if (Field[x, row] != null)
            {
                Field[x, row - 1] = Field[x, row];
                Field[x, row] = null;
                Field[x, row - 1].position += Vector3.down;
            }
        }
    }

    public static void decreaseAllRowsAbove(int row)
    {
        for (int i = row; i<h; i++)
        {
            decreaseRow(i);
        }
    }


    public static bool isRowFull(int row)
    {
        for(int x = 0; x < w; x++)
        {
            if (Field[x,row] == null)
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsBlockAbovePlayZone()
    {
        for (int x = 0; x < w; x++)
        {
            if (Field[x, 20] != null)
            {
                return true;
            }
        }
        return false;
    }


    public static void deleteFullRows()
    {
        for(int y=0; y < h; y++)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                decreaseAllRowsAbove(y + 1);
                y--;
            }
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
