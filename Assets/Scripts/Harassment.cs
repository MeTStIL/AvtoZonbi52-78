using DefaultNamespace;
using UnityEngine;

public class Harassment : MonsterTypeIdentifier
{
    public void Update()
    {
        TryMakeHarassment();
    }

    private void TryMakeHarassment()
    {
        if (other.IsStatic) return;
        if (Vector3.Distance(transform.position, other.player.position) <= other.VisibleRadius / 2)
            other.currentSpeed = 0;
        else if (Vector3.Distance(transform.position, other.player.position) <= other.VisibleRadius)
            StartHarassment();
        else if (Vector3.Distance(transform.position, other.player.position) > other.chaseRadius)
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