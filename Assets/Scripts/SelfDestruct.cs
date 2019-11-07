using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Delete();
    }

    void Delete()
    {
        if (transform.childCount == 0 || transform.position.y <= -100)
        {
            Destroy(gameObject);
        }
    }
}
