using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isAlive = false;
    public int neighbours = 0;

    public void SetAlive(bool alive)
    {
        isAlive = alive;

        if (alive)
        {
            GetComponent<Renderer>().enabled = true;
        }
        else
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
}
