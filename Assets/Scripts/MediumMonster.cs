public class MediumMonster : Monster
{
    public override void Awake()
    {
        base.Awake();
        MonsterInfo = new MonsterInfo()
        {
            LivesCount = 2,
            StandardSpeed = 2,
            SpeedAttack = 4,
            CurrentSpeed = 2
        };
        ButtonCount = 3;
    }
}
