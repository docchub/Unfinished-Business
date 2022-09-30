using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    Sound music;
    
    private void Start()
    {
        FindObjectOfType<AudioManager>().PlaySound(music.name);
    }
}
