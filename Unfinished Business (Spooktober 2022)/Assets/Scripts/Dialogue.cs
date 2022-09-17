using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    // Fields
    List<string> name;
    List<string> sentences;

    // Constructor
    public Dialogue()
    {
        name = new List<string>();
        sentences = new List<string>();
    }

    // Property
    public List<string> Name
    {
        get { return name; }
        set { name = value; }
    }
    public List<string> Sentences
    {
        get { return sentences; }
        set { sentences = value; }
    }
}
