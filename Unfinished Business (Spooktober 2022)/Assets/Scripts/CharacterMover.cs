using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    [SerializeField]
    GameObject[] characters;

    private GameObject instantiatedChar = null;
    private List<GameObject> sceneCharacters = new List<GameObject>();

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
                sceneCharacters.Add(instantiatedChar);

                // Fade in
                FindObjectOfType<FadeInOut>().FadeElement(true, instantiatedChar);
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
            // Fade out
            FindObjectOfType<FadeInOut>().FadeElement(false, instantiatedChar);            
        }
    }

    /// <summary>
    /// Empties the list of instantiated characters
    /// </summary>
    public void ClearSceneCharacters()
    {
        sceneCharacters.Clear();
    }
}
