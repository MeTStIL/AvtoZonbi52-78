using System.Collections;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] objectSounds;
    private AudioSource AudioSource => GetComponent<AudioSource>();
    private bool isPaused;

    public void PlaySound(AudioClip clip, float volume = 1f, float p1 = 0.85f, float p2 = 1.2f, float fadeInTime = 1f)
    {
        AudioSource.pitch = Random.Range(p1, p2);
        AudioSource.PlayOneShot(clip, volume);
        if (fadeInTime != 0f)
            StartCoroutine(FadeIn(fadeInTime, volume));
    }

    protected void StopSound()
    {
        AudioSource.Stop();
    }

    protected void PauseMusic()
    {
        AudioSource.Pause();
    }

    protected void ResumeMusic()
    {
        AudioSource.UnPause();
    }
    
    private IEnumerator FadeIn(float fadeInTime, float targetVolume)
    {
        AudioSource.volume = 0f;
        while (AudioSource.volume < targetVolume)
        {
            AudioSource.volume += Time.deltaTime / fadeInTime;
            yield return null;
        }
    }
}
