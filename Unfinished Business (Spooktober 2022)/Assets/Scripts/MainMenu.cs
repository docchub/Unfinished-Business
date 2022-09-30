using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    static bool bev1complete = false;
    static bool bev2complete = false;
    static bool bev3complete = false;

    static bool nas1complete = false;
    static bool nas2complete = false;
    static bool nas3complete = false;

    static bool ms1complete = false;
    static bool ms2complete = false;
    static bool ms3complete = false;

    [SerializeField]
    GameObject alertText;

    [SerializeField]
    GameObject finishedAlertText;

    [SerializeField]
    Vector2[] cooridinates;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("You quit the game.");
        Application.Quit();
    }

    public void PlayBeverlee()
    {
        if (!bev1complete)
        {
            bev1complete = true;
            SceneManager.LoadScene("Bev1");
        }
        else if (!bev2complete)
        {
            bev2complete = true;
            SceneManager.LoadScene("Bev2");
        }
        else if (!bev3complete)
        {
            if (nas3complete)
            {
                bev3complete = true;
                SceneManager.LoadScene("Bev3");
            }
            else
            {
                LockedRouteAlert(0);
            }
        }
        else
        {
            FinishedRouteAlert(0);
        }
    }

    public void PlayNastasia()
    {
        if (!nas1complete)
        {
            nas1complete = true;
            SceneManager.LoadScene("Nas1");
        }
        else if (!nas2complete)
        {
            nas2complete = true;
            SceneManager.LoadScene("Nas2");
        }
        else if (!nas3complete)
        {
            nas3complete = true;
            SceneManager.LoadScene("Nas3");
        }
        else
        {
            FinishedRouteAlert(1);
        }
    }

    public void PlayMS()
    {
        if (!ms1complete)
        {
            ms1complete = true;
            SceneManager.LoadScene("MS1");
        }
        else if (!ms2complete)
        {
            ms2complete = true;
            SceneManager.LoadScene("MS2");
        }
        else if (!ms3complete)
        {
            ms3complete = true;
            SceneManager.LoadScene("MS3");
        }
        else
        {
            FinishedRouteAlert(2);
        }
    }

    public void PlayCorduroy()
    {
        if (bev3complete && nas3complete && ms3complete)
        {
            SceneManager.LoadScene("Corduroy");
        }
        else
        {
            LockedRouteAlert(3);
        }
    }

    void FinishedRouteAlert(int index)
    {
        Instantiate(finishedAlertText, cooridinates[index], Quaternion.identity, transform);
    }

    void LockedRouteAlert(int index)
    {
        Instantiate(alertText, cooridinates[index], Quaternion.identity, transform);
    }
}
