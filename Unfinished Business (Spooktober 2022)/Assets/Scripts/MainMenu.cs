using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    static bool nas1complete = false;
    static bool nas2complete = false;
    static bool nas3complete = false;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("You quit the game.");
        Application.Quit();
    }

    public void PlayNastasia()
    {
        if (!nas1complete)
        {
            nas1complete = true;
            Debug.Log("Went to go to scene = nas1");
            SceneManager.LoadScene("Nas1");
        }
        else if (!nas2complete)
        {
            nas2complete = true;
            Debug.Log("Attempted to go to scene = nas2");
            SceneManager.LoadScene("Nas2");
        }
        else if (!nas3complete)
        {
            nas3complete = true;
            Debug.Log("Attempted to go to scene = nas3");
            SceneManager.LoadScene("Nas3");
        }
    }
}
