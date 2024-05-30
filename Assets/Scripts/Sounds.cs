using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] objectSounds;
    private AudioSource audioSource => GetComponent<AudioSource>();
    private bool isPAused = false;

    public void PlaySound(AudioClip clip, float volume = 1f, float p1 = 0.85f, float p2 = 1.2f, float fadeInTime = 1f)
    {
        audioSource.pitch = Random.Range(p1, p2);
        audioSource.PlayOneShot(clip, volume);
        if (fadeInTime != 0f)
            StartCoroutine(FadeIn(fadeInTime, volume));
    }

    public void StopSound()
    {
        audioSource.Stop();
    }
    
    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void ResumeMusic()
    {
        audioSource.UnPause();
    }


    private IEnumerator FadeIn(float fadeInTime, float targetVolume)
    {
        audioSource.volume = 0f;
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += Time.deltaTime / fadeInTime;
            yield return null;
        }
    }
}
