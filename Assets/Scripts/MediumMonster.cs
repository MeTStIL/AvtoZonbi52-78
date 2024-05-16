using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumMonster : Monster
{
    public override void Awake()
    {
        base.Awake();
        LivesCount = 2;
        StandardSpeed = 2;
        speedAttack = 4;
        //WalkingRadius = 6;
        //VisibleRadius = 7;
        ButtonCount = 3;
        obstacleLayer = LayerMask.GetMask("Collision");
        currentSpeed = StandardSpeed;
    }
}
