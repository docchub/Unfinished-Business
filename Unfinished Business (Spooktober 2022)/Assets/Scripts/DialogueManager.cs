using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class DialogueManager : MonoBehaviour
{
    // ------------------------------------------------------------------------
    // PUBLIC FIELDS ----------------------------------------------------------
    // ------------------------------------------------------------------------

    // Creates a dialogue object
    // Currently the class and its content can be edited in the unity inspector
    public Dialogue dialogue;

    // Attach typefaces in the inspector
    public TextMeshPro nameText;
    public TextMeshPro dialogueText;

    // ------------------------------------------------------------------------
    // PRIVATE FIELDS ---------------------------------------------------------
    // ------------------------------------------------------------------------

    // Creates a queue to store string inputs
    private Queue<string> speakers;
    private Queue<string> sentences;

    // Queue holding data read in from text files
    // as strings representing different audio files
    private Queue<string> audiolines;

    // Holds the speaking character
    private Queue<string> characters;

    // Queue of backgrounds
    private Queue<string> background;

    // Track previous name
    private string prevSpeaker;

    // Time the manager waits before typing next character
    private float typingSpeed = 0.04f;

    // Is the coroutine running?
    private bool crRunning = false;

    // Number of pieces to a line in a text file
    private const int parts = 4;

    // Testing File Path
    private string path;

    // Intro Scene File Paths
    private const int introScenes = 3;
    private string sceneDataPath;
    private Queue<string> introSceneFilePaths;
    private bool loadStarted = false;

    // Clicking
    private bool prevMouseState;

    // ---- Scene Selection ----
    [SerializeField]
    string sceneFilePath;
    private string path0;

    // SFX
    string currentSpeaker;
    bool playAudio;

    [SerializeField]
    Sound[] sounds;

    // Auto mode
    static bool auto;

    // ------------------------------------------------------------------------
    // METHODS ----------------------------------------------------------------
    // ------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the queues for names, audio, and dialogue
        speakers = new Queue<string>();
        audiolines = new Queue<string>();
        characters = new Queue<string>();
        background = new Queue<string>();
        sentences = new Queue<string>();

        dialogue = new Dialogue();

        // Ready blip sfx
        foreach (Sound s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.clip;

            s.Source.volume = s.volume;
            s.Source.pitch = s.pitch;
        }

        //// Testing path
        //path = Application.dataPath + "/testscene.txt";

        // Special Scene Path
        if (sceneFilePath != null && sceneFilePath != "")
        {
            path0 = Application.dataPath + "/StreamingAssets/" + sceneFilePath;
        }

        // Read in scene data
        sceneDataPath = Application.dataPath + "/StreamingAssets/introscenedata.txt";
        introSceneFilePaths = new Queue<string>();
        ReadScenePaths();

        // File IO
        ReadScene(dialogue);

        // Slow start-up
        StartDialogue(dialogue);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !loadStarted)
        {
            //Debug.Log("You clicked.");
            DisplayNextSentence();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("enabled/disabled automode");
            auto = !auto;
            DisplayNextSentence();
        }
    }

    /// <summary>
    /// Initiates a dialogue queue
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(Dialogue dialogue)
    {
        // Clear queue
        sentences.Clear();

        // Enqueue all names
        foreach (string name in dialogue.Name)
        {
            speakers.Enqueue(name);
        }

        // Enqueue all sentences
        foreach (string sentence in dialogue.Sentences)
        {
            sentences.Enqueue(sentence);
        }

        // ----- Backgrounds -----
        FindObjectOfType<Backgrounds>().DrawBackground(background.Dequeue());

        DisplayNextSentence();
    }

    /// <summary>
    /// Reads each queued sentence until the queue is empty
    /// </summary>
    public void DisplayNextSentence()
    {        
        // If the coroutine is running and you click
        if (crRunning)
        {
            typingSpeed = 0.005f;
        }

        // Display nothing
        else if (!crRunning && sentences.Count == 0)
        {
            if (sceneFilePath != "" || introSceneFilePaths.Count == 0)
            {
                FindObjectOfType<MainMenu>().SaveGame();
                SceneManager.LoadScene("SceneSelector");
            }
            
            EndDialogue();
            return;
        }

        // Dequeue and read sentence in debugger
        else if (!crRunning)
        {
            // Reset typing speed
            typingSpeed = 0.04f;

            // Blip References
            currentSpeaker = speakers.Peek();

            // Only play blips when no voice acting
            if (audiolines.Peek() == "")
            {
                playAudio = true;
            }
            else
            {
                playAudio = false;
            }

            // Change to weirdscape when...
            if (currentSpeaker == "???" && sceneFilePath == "corduroyfinale.txt")
            {
                FindObjectOfType<Backgrounds>().DrawBackground("weirdscape");
            }

            // Voice acting
            PlayAudio();

            // Characters
            if (characters.Peek() != "")
            {
                PlaceCharacter();
            }
            else
            {
                characters.Dequeue();
            }

            // Get the next name and text to display
            nameText.text = speakers.Dequeue();
            string sentence = sentences.Dequeue();
            dialogueText.text = sentence;

            // Calls IEnumerator
            crRunning = true;
            StartCoroutine(TypeSentence(sentence));
        }
    }

    /// <summary>
    /// Loops through each letter in a string pulled from the queue
    /// </summary>
    /// <param name="sentence"></param>
    /// <returns></returns>
    IEnumerator TypeSentence (string sentence)
    {        
        dialogueText.text = "";
        int counter = 1;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            if (typingSpeed >= 0.04f)
            {
                if (counter % 3 == 0)
                {
                    PlayBlip();
                }
            }
            else
            {
                if (counter % 6 == 0)
                {
                    PlayBlip();
                }
            }
            counter++;

            // waits a single frame
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        if (auto)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            crRunning = false;
            DisplayNextSentence();
        }
        else
        {
            // Call once sentence is spelled out
            crRunning = false;
            StopAllCoroutines();
        }
    }

    void PlayBlip()
    {
        if (playAudio)
        {
            if (currentSpeaker == "Bev" || currentSpeaker == "Beverlee" || (currentSpeaker == "???" && sceneFilePath == "bev1.txt"))
            {
                sounds[0].Source.Play();
            }
            else if (currentSpeaker == "Nas" || currentSpeaker == "Nastasia")
            {
                sounds[1].Source.Play();
            }
            else if (currentSpeaker == "MS")
            {
                sounds[2].Source.Play();
            }
            else if (currentSpeaker == "Corduroy" || (currentSpeaker == "???" && sceneFilePath == ""))
            {
                sounds[3].Source.Play();
            }
            else if (currentSpeaker == "???" && sceneFilePath == "corduroyfinale.txt")
            {
                sounds[4].Source.Play();
            }
            else
            {
                sounds[5].Source.Play();
            }
        }
    }

    /// <summary>
    /// Announces end of dialogue queue for debugging purposes
    /// </summary>
    void EndDialogue()
    {
        Debug.Log("End of conversation.");

        if (introSceneFilePaths.Count > 0 && !loadStarted)
        {
            loadStarted = true;
            NewDialogue(false);
        }
    }

    /// <summary>
    /// Gets the filepaths of the introductory scenes
    /// </summary>
    void ReadScenePaths()
    {
        StreamReader reader = null;
        string line;
        int lineNumber = 0;

        // Attempt to read in a scene from a text file
        try
        {
            reader = new StreamReader(sceneDataPath);

            while ((line = reader.ReadLine()) != null)
            {
                introSceneFilePaths.Enqueue(Application.dataPath + "/StreamingAssets/" + line);
                lineNumber++;
            }
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

    /// <summary>
    /// Read in data from a text file
    /// </summary>
    /// <param name="dialogue"></param>
    void ReadScene(Dialogue dialogue)
    {
        StreamReader reader = null;
        string line;
        string[] sceneData = new string[parts];
        int count = 1;

        // Attempt to read in a scene from a text file
        try
        {
            if (sceneFilePath != null && sceneFilePath != "")
            {
                reader = new StreamReader(path0);
            }
            else
            {
                reader = new StreamReader(introSceneFilePaths.Dequeue());
            }

            // Read the background at the top of the text file
            line = reader.ReadLine();
            background.Enqueue(line);

            while ((line = reader.ReadLine()) != null)
            {
                sceneData = line.Split('|');

                count++;

                // Assign values based on read in data
                // 1. speaker name
                // 2. audio file
                // 3. sentence
                // 4. character
                dialogue.Name.Add(sceneData[0]);
                audiolines.Enqueue(sceneData[1]);
                characters.Enqueue(sceneData[2]);
                dialogue.Sentences.Add(sceneData[3]);

                //// Debugger
                //Debug.Log("Read in: " + sceneData[0] + " from line " + String.Format("" + count));
                //Debug.Log("Read in: " + sceneData[1] + " from line " + String.Format("" + count));
                //Debug.Log("Read in: " + sceneData[2] + " from line " + String.Format("" + count));
                //Debug.Log("Read in: " + sceneData[3] + " from line " + String.Format("" + count));
            }
        }

        // Writes exceptions to the Output window
        catch (IOException ioe)
        {
            Debug.Log("IO Error: " + ioe.Message);

            dialogue.Name.Add("File IO Error");
            dialogue.Sentences.Add("did not find scene");
        }
        catch (Exception ex)
        {
            Debug.Log("General Error: " + ex.Message);

            dialogue.Name.Add("Somethine Else");
            dialogue.Sentences.Add("did not find scene because some other thing happened");
        }

        // Close the reader
        if (reader != null)
        {
            reader.Close();
        }
    }

    /// <summary>
    /// Plays audio files during scene
    /// </summary>
    void PlayAudio()
    {
        // Say BOO if a new character is talking
        if (audiolines.Peek() != prevSpeaker || prevSpeaker == null)
        {
            //// Stop any audio when the first audio in the queue plays
            //if (prevSpeaker != null)
            //{
            //    FindObjectOfType<AudioManager>().StopSound(prevSpeaker);
            //}

            prevSpeaker = audiolines.Peek();

            if (audiolines.Peek() != "")
            {
                FindObjectOfType<AudioManager>().PlaySound(audiolines.Dequeue());
            }
            else
            {
                audiolines.Dequeue();
            }
        }

        // Cycle through the Queue without playing audio
        else
        {
            prevSpeaker = audiolines.Peek();
            audiolines.Dequeue();
        }
    }

    /// <summary>
    /// Control Character Appearance
    /// </summary>
    void PlaceCharacter()
    {
        if (characters.Peek() == "empty")
        {
            FindObjectOfType<CharacterMover>().MoveOffScreen();
            characters.Dequeue();
        }
        else if (characters.Peek() != null)
        {
            // Remove any previous character
            FindObjectOfType<CharacterMover>().MoveOffScreen();

            // Place a character in the scene
            Debug.Log("Placed character.");
            FindObjectOfType<CharacterMover>().MoveOnScreen(characters.Dequeue());
        }
    }

    /// <summary>
    /// Start a new dialogue scene
    /// </summary>
    public void NewDialogue(bool loaded)
    {
        if (!loaded || !loadStarted)
        {            
            // Fade to black
            FindObjectOfType<FadeInOut>().FadeScene(true);

            // Clear data
            FindObjectOfType<CharacterMover>().ClearSceneCharacters();
            dialogue.Name.Clear();
            dialogue.Sentences.Clear();
            audiolines.Clear();
            characters.Clear();
            background.Clear();
        }
        else
        {            
            FindObjectOfType<FadeInOut>().FadeScene(false);

            // File IO
            ReadScene(dialogue);

            // Starts the dialogue tree
            StartDialogue(dialogue);

            loadStarted = false;
        }
    }
}
