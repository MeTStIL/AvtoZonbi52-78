using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumMonster : Monster
{
    public override void Awake()
    {
        base.Awake();
        LivesCount = Random.Range(1, 3);
        StandardSpeed = 2;
        WalkingRadius = 6;
        obstacleLayer = LayerMask.GetMask("Collision");
    }
}
