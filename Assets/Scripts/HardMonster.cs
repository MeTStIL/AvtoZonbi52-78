using UnityEngine;

public class HardMonster : Monster
{
    public override void Awake()
    {
        base.Awake();
        LivesCount = 3;
        StandardSpeed = 2;
        obstacleLayer = LayerMask.GetMask("Collision");
        speedAttack = 3;
        ButtonCount = 4;
        currentSpeed = StandardSpeed;
    }
}
