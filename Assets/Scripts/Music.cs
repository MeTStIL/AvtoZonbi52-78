using System.Collections;
using UnityEngine;

public class Music : MonsterTypeIdentifier
{
    private bool isSound;
    
    public void Update()
    {
        CheckForDistance();
    }

    public void LateUpdate()
    {
        TryMakeSoundOnClick();
    }

    private void CheckForDistance()
    {
        if (!(Vector3.Distance(transform.position, other.player.position) <= 2f) || isSound) return;
        isSound = true;
        other.PlaySound(other.objectSounds[3]);
        StartCoroutine(StopSound());
    }

    private IEnumerator StopSound()
    {
        yield return new WaitForSeconds(other.objectSounds[3].length);
        isSound = false;
    }

    private void TryMakeSoundOnClick()
    {
        switch (other.IsCorrectClick)
        {
            case null:
                return;
            case true:
                other.PlaySound(other.objectSounds[2], volume: 0.5f, fadeInTime: 0);
                break;
            default:
                other.PlaySound(other.objectSounds[0], volume: 0.5f, fadeInTime: 0);
                break;
        }
    }
}