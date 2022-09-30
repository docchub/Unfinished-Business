using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    static bool intro = false;
    
    static bool bev1complete = false;
    static bool bev2complete = false;
    static bool bev3complete = false;

    static bool nas1complete = false;
    static bool nas2complete = false;
    static bool nas3complete = false;

    static bool ms1complete = false;
    static bool ms2complete = false;
    static bool ms3complete = false;

    static bool theEnd = false;

    string path;

    [SerializeField]
    GameObject alertText;

    [SerializeField]
    GameObject finishedAlertText;

    Vector2 cooridinates;

    GameObject prevAlert;

    private void Start()
    {
        cooridinates = new Vector2(0f, -3.4f);
        path = Application.dataPath + "/StreamingAssets/savefile.txt";
        LoadGame();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("UB");
    }

    public void QuitGame()
    {
        Debug.Log("You quit the game.");
        Application.Quit();
    }

    public void SaveGame()
    {
        StreamWriter writer = null;
        string line;

        // Attempt to read in a scene from a text file
        try
        {
            writer = new StreamWriter(path);

            line = String.Format(intro + "|" + bev1complete + "|" + bev2complete + "|" + bev3complete + "|" +
                                               nas1complete + "|" + nas2complete + "|" + nas3complete + "|" +
                                               ms1complete + "|" + ms2complete + "|" + ms3complete);
            Debug.Log(line);

            writer.Write(line);
        }

        // Writes exceptions to the Output window
        catch (IOException ioe)
        {
            Debug.Log("IO Error: " + ioe.Message);
        }
        catch (Exception ex)
        {
            Debug.Log("General Error: " + ex.Message);
        }

        // Close the reader
        if (writer != null)
        {
            writer.Close();
        }
    }

    void LoadGame()
    {
        StreamReader reader = null;
        string line;
        string[] lines;
        bool[] boolLines = new bool[11];

        // Attempt to read in a scene from a text file
        try
        {
            reader = new StreamReader(path);
            line = reader.ReadLine();
            lines = line.Split('|');

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "false" || lines[i] == "False")
                {
                    boolLines[i] = false;
                }
                else
                {
                    boolLines[i] = true;
                }
            }

            intro = boolLines[0];
            bev1complete = boolLines[1];
            bev2complete = boolLines[2];
            bev3complete = boolLines[3];
            nas1complete = boolLines[4];
            nas2complete = boolLines[5];
            nas3complete = boolLines[6];
            ms1complete = boolLines[7];
            ms2complete = boolLines[8];
            ms3complete = boolLines[9];
        }

        // Writes exceptions to the Output window
        catch (IOException ioe)
        {
            Debug.Log("IO Error: " + ioe.Message);
        }
        catch (Exception ex)
        {
            Debug.Log("General Error: " + ex.Message);
        }

        // Close the reader
        if (reader != null)
        {
            reader.Close();
        }
    }

    void FinishedRouteAlert()
    {
        if (prevAlert != null)
        {
            Destroy(prevAlert);
        }

        prevAlert = Instantiate(finishedAlertText, cooridinates, Quaternion.identity, transform);
    }

    void LockedRouteAlert()
    {
        if (prevAlert != null)
        {
            Destroy(prevAlert);
        }

        prevAlert = Instantiate(alertText, cooridinates, Quaternion.identity, transform);
    }

    public void PlayBeverlee()
    {
        intro = true;
        
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
                LockedRouteAlert();
            }
        }
        else
        {
            FinishedRouteAlert();
        }
    }

    public void PlayNastasia()
    {
        intro = true;

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
            FinishedRouteAlert();
        }
    }

    public void PlayMS()
    {
        intro = true;

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
            FinishedRouteAlert();
        }
    }

    public void PlayCorduroy()
    {
        if (theEnd)
        {
            FinishedRouteAlert();
        }
        else if (bev3complete && nas3complete && ms3complete)
        {
            theEnd = true;
            SceneManager.LoadScene("Corduroy");
        }
        else
        {
            LockedRouteAlert();
        }
    }
}
