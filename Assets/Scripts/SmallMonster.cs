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
        speedAttack = 5;
        ButtonCount = 2;
        currentSpeed = StandardSpeed;
    }
    
}
