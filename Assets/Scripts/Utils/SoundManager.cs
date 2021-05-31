using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public struct Sounds
    {
        public string name;
        public AudioClip sound;
    }

    public Sounds[] sounds;
    public static SoundManager instance;
    public AudioSource source;

    public void PlaySound(string soundKey)
    {

        source.PlayOneShot(findIndexOfSound(soundKey));
    }

    private AudioClip findIndexOfSound(string soundKey)
    {
        foreach (Sounds sound in sounds)
        {
            if (sound.name == soundKey)
            {
                return sound.sound;
            }
        }
        return null;
    }

}
