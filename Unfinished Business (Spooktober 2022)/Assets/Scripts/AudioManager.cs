using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.clip;

            s.Source.volume = s.volume;
            s.Source.pitch = s.pitch;
        }
    }

    /// <summary>
    /// Plays a sound from the loaded sounds in the Sound class
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
        if (name != "")
        {
            foreach (Sound s in sounds)
            {
                if (s.name == name)
                {
                    if (s.name == "ubmaintheme")
                    {
                        s.Source.loop = true;
                    }
                    s.Source.Play();
                    return;
                }
            }
            // if no sound found...
            Debug.Log("Could not find the audio.");
        }
    }

    public void StopSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.Source.Stop();
                return;
            }
        }

        // if no sound found...
        Debug.Log("Could not find the audio to stop.");
    }
}
