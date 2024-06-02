public class SmallMonster : Monster
{
    public override void Awake()
    {
        base.Awake();
        MonsterInfo = new MonsterInfo()
        {
            LivesCount = 1,
            StandardSpeed = 2,
            SpeedAttack = 5,
            CurrentSpeed = 2
        };
        ButtonCount = 2;
    }
}
