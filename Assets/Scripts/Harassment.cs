using System;
using DefaultNamespace;
using UnityEngine;

public class Harassment : MonsterTypeIdentifier
{
    public float chaseRadius;

    private void Start()
    {
        chaseRadius = 3f * other.visibleRadius;
    }

    public void Update()
    {
        TryMakeHarassment();
    }

    private void TryMakeHarassment()
    {
        if (other.isStatic) return;
        if (Vector3.Distance(transform.position, other.player.position) <= other.visibleRadius / 2)
            other.currentSpeed = 0;
        else if (Vector3.Distance(transform.position, other.player.position) <= other.visibleRadius)
            StartHarassment();
        else if (Vector3.Distance(transform.position, other.player.position) > chaseRadius)
            StopHarassment();
    }

    private void StartHarassment()
    {
        other.isHarassment = true;
        other.currentSpeed = other.speedAttack;
        other.targetPosition = other.player.position;
    }

    private void StopHarassment()
    {
        other.isHarassment = false;
        other.currentSpeed = other.StandardSpeed;
        other.SetNewTargetPosition();
    }
}