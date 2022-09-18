using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField]
    GameObject[] characters;

    //private List<GameObject> instantiatedCharacters = new List<GameObject>();
    private GameObject instantiatedChar = null;

    /// <summary>
    /// Instantiate a given character sprite
    /// </summary>
    /// <param name="character"></param>
    public void MoveOnScreen(string character)
    {
        foreach (GameObject c in characters)
        {
            if (c.name == character)
            {
                instantiatedChar = Instantiate(c);
            }
        }        
    }

    /// <summary>
    /// Destroy an instantiated character sprite
    /// </summary>
    /// <param name="character"></param>
    public void MoveOffScreen()
    {
        if (instantiatedChar != null)
        {
            Destroy(instantiatedChar);
        }
        
        //foreach (GameObject g in instantiatedCharacters)
        //{
        //    if (g.name == character + "(Clone)")
        //    {
        //        Destroy(g);
        //        return;
        //    }
        //}
    }
}
