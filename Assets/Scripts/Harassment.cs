using System;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class Harassment : MonsterTypeIdentifier
{
    public float chaseRadius;
    private FightingMechanic fightingMechanic;

    private void Start()
    {
        chaseRadius = 1.3f * other.visibleRadius;
        fightingMechanic = gameObject.GetComponent<FightingMechanic>();
    }

    public void Update()
    {
        TryMakeHarassment();
    }

    private void TryMakeHarassment()
    {
        if (other.isStatic) return;
        if (Vector3.Distance(transform.position, other.player.position) <= other.visibleRadius / 2)
            other.MonsterInfo.CurrentSpeed = 0;
        else if (Vector3.Distance(transform.position, other.player.position) <= other.visibleRadius)
            StartHarassment();
        else if (Vector3.Distance(transform.position, other.player.position) > chaseRadius)
            StopHarassment();
    }

    private void StartHarassment()
    {
        other.isHarassment = true;
        other.MonsterInfo.CurrentSpeed = other.MonsterInfo.SpeedAttack;
        other.targetPosition = other.player.position;
    }

    private void StopHarassment()
    {
        other.isHarassment = false;
        other.MonsterInfo.CurrentSpeed = other.MonsterInfo.StandardSpeed;
        other.SetNewTargetPosition();
        fightingMechanic.timeForAttack = Time.time;
        fightingMechanic.DestroyButtons();
    }
}