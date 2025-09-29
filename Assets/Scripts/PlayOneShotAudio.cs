using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOneShotAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footstepSound;

    public void PlayFootstepSound()
    {
        audioSource.PlayOneShot(footstepSound);
    }
}
