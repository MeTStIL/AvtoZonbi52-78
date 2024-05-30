using System;
using System.Collections;
using DefaultNamespace;
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
        if (other.IsCorrectClick != null)
            if (other.IsCorrectClick == true)
                other.PlaySound(other.objectSounds[2], volume: 0.5f, fadeInTime: 0);
            else
                other.PlaySound(other.objectSounds[0], volume: 0.5f, fadeInTime: 0);
    }
}