using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgrounds : MonoBehaviour
{
    [SerializeField]
    private GameObject[] background;

    private GameObject currentBG = null;

    public void DrawBackground(string bg)
    {
        // Only draw a background on the first sentence or when it changes
        if (currentBG == null || currentBG.name != bg)
        {
            foreach (GameObject b in background)
            {
                if (b.name == bg)
                {
                    Destroy(currentBG);
                    currentBG = Instantiate(b);
                }
            }
        }
    }
}
