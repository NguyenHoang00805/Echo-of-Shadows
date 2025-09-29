using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }
    public AudioSource introSource, loopSource;

    // Start is called before the first frame update
    void Start()
    {
        introSource.Play();
        loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
    }

    //public void ChangeMusicVolumn(float _change)
    //{
    //    float currentVolume = PlayerPrefs.GetFloat("musicVolume");
    //    currentVolume += _change;

    //    if (currentVolume > 1)
    //        currentVolume = 0;
    //    else if (currentVolume < 0)
    //        currentVolume = 1;

    //    introSource.volume = currentVolume;
    //    loopSource.volume = currentVolume;

    //    PlayerPrefs.SetFloat("musicVolume",currentVolume);
    //}
}
