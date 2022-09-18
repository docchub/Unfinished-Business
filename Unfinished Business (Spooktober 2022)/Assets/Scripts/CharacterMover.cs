using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField]
    GameObject[] characters;

    public void MoveOnScreen(string character)
    {
        foreach (GameObject c in characters)
        {
            if (c.name == character)
            {
                Instantiate(c);
            }
        }        
    }

    public void MoveOffScreen(string character)
    {
        foreach (GameObject c in characters)
        {
            if (c.name == character)
            {
                Destroy(c);
            }
        }
    }
}
