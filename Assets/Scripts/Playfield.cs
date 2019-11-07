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

    public static bool insideVerticalBorder(Vector2 position)
    {
        return (int)position.x >= 0 && (int)position.x < w;
    }

    public static void deleteAll()
    {
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (Field[x, y] != null)
                {
                    deletePosition(x, y);
                }
            }
        }
    }


    public static void deleteRow(int row)
    {
        for (int x = 0; x < w; x++)
        {
            deletePosition(x, row);
        }

        FindObjectOfType<GameManager>().IncrementCurrentLines();
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
        for (int i = row; i < h; i++)
        {
            decreaseRow(i);
        }
    }

    public static void decreaseAllColumns()
    {
        for (int i = 0; i < w; i++)
        {
            decreaseColumn(i);
        }
    }

    public static void decreaseColumn(int c)
    {
        for (int i = 0; i < h; i++)
        {
            if (EmptyAbove(c, i))
            {
                break;
            }
            else
            {
                while (Field[c, i] == null)
                {
                    DecreaseBlocksAbove(c, i);
                }
            }
        }
    }

    public static void DecreaseBlocksAbove(int c, int r)
    {
        for (int i = r + 1; i < h; i++)
        {
            if (Field[c, i] != null)
            {
                Field[c, i - 1] = Field[c, i];
                Field[c, i] = null;
                Field[c, i - 1].position += Vector3.down;
            }
        }
    }

    public static bool EmptyAbove(int c, int row)
    {
        if (row + 1 > +h) return true;
        for (int y = row + 1; y < h; y++)
        {
            if (Field[c, y] != null) return false;
        }

        return true;
    }


    public static bool isRowFull(int row)
    {
        for (int x = 0; x < w; x++)
        {
            if (Field[x, row] == null)
            {
                return false;
            }
        }
        return true;
    }

    public static bool isRowEmpty(int row)
    {
        for (int x = 0; x < w; x++)
        {
            if (Field[x, row] != null)
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
        /*
        for(int y=0; y < h; y++)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                decreaseAllRowsAbove(y + 1);
                y--;
            }
        }
        */
        Stack<int> RowsToDelete = new Stack<int>();

        for (int y = 0; y < h; y++)
        {
            if (isRowFull(y))
            {
                deleteRow(y);
                RowsToDelete.Push(y);
            }
        }

        while (RowsToDelete.Count != 0)
        {
            
            decreaseAllRowsAbove(RowsToDelete.Pop() + 1);
        }
        


    }

    public static void deletePosition(int x, int y)
    {
        Field[x, y].gameObject.GetComponent<Block>().Clear();
        Field[x, y] = null;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void insertLines(int rows, GameObject block)
    {
        for(int i = 0; i < rows; i++)
        {
            float prob = Random.value;
            for(int j = 0; j < w; j++)
            {
                if(prob >= ((float) j / (float) w) && prob <= ((float)(j +1) / (float)w))
                {
                    continue;
                }
                Field[j, i] = Instantiate(block, new Vector3(j,i, 0), Quaternion.identity).transform;
            }
        }
    }
}
