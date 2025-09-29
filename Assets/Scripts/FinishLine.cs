using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{

    public string playerTag = "Player";
    public GameObject finishUI;
    public AudioSource victorySFX;
    public static MusicPlayer Instance { get; private set; }
    public AudioSource introSource, loopSource;
    public float fadeOutTime = 1f;

    private void Awake()
    {
        finishUI.SetActive(false);
    }

    void Start()
    {
        introSource.Play();
        loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
    }

    public void FadeOutMusic()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float startVolume = loopSource.volume;

        while (loopSource.volume > 0)
        {
            loopSource.volume -= startVolume * Time.deltaTime / fadeOutTime;
            yield return null;
        }

        loopSource.Stop();
        loopSource.volume = startVolume; // Reset volume to original in case you want to fade in later.
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            if (finishUI != null)
            {
                FadeOutMusic();
                finishUI.SetActive(true);
                victorySFX.Play();
            }
        }
    }
}
