using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    private Queue<string> audioToPlay;

    // Track previous name
    private string prevSpeaker;

    // Time the manager waits before typing next character
    private float typingSpeed = 0.1f;

    // Is the coroutine running?
    private bool crRunning = false;

    // Number of pieces to a line in a text file
    const int parts = 3;

    string path;

    // ------------------------------------------------------------------------
    // METHODS ----------------------------------------------------------------
    // ------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the queues for names, audio, and dialogue
        speakers = new Queue<string>();
        audioToPlay = new Queue<string>();
        sentences = new Queue<string>();

        dialogue = new Dialogue();
        path = Application.dataPath + "/Scripts/textfiles/testscene.txt";

        // File IO
        ReadScene(dialogue);

        // Starts the dialogue tree
        StartDialogue(dialogue);
    }

    /// <summary>
    /// Initiates a dialogue queue
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting dialogue with " + dialogue.Name);

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
            typingSpeed = 0.001f;
        }

        // Display nothing
        else if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // Dequeue and read sentence in debugger
        else if (!crRunning)
        {
            // Reset typing speed
            typingSpeed = 0.05f;

            // Say BOO if a new character is talking
            if (audioToPlay.Peek() != prevSpeaker || prevSpeaker == null)
            {
                // Don't try to stop any audio when the first audio in the queue plays
                if (prevSpeaker != null)
                {
                    FindObjectOfType<AudioManager>().StopSound(prevSpeaker);
                }

                prevSpeaker = audioToPlay.Peek();
                FindObjectOfType<AudioManager>().PlaySound(audioToPlay.Dequeue());

                // Place a character in the scene
                FindObjectOfType<CharacterMover>().MoveOnScreen(prevSpeaker);
            }

            // Cycle through the Queue without playing audio
            else
            {
                prevSpeaker = audioToPlay.Peek();
                audioToPlay.Dequeue();
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

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            // waits a single frame
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        // Call once sentence is spelled out
        crRunning = false;
        StopAllCoroutines();
    }

    /// <summary>
    /// Announces end of dialogue queue for debugging purposes
    /// </summary>
    void EndDialogue()
    {
        Debug.Log("End of conversation.");
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

        // track how many sentences in queue
        int sCount = 0;

        // Attempt to read in a scene from a text file
        try
        {
            reader = new StreamReader(path);

            while ((line = reader.ReadLine()) != null)
            {
                sceneData = line.Split('|');

                // Assign values based on read in data
                // 1. speaker name
                // 2. audio file
                // 3. sentence
                dialogue.Name.Add(sceneData[0]);
                audioToPlay.Enqueue(sceneData[1]);
                dialogue.Sentences.Add(sceneData[2]);

                sCount++;
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
}
