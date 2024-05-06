using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMonster : Monster
{
    public override void Awake()
    {
        base.Awake();
        LivesCount = 1;
        StandardSpeed = 2;
        WalkingRadius = 6;
        obstacleLayer = LayerMask.GetMask("Collision");
    }
}
