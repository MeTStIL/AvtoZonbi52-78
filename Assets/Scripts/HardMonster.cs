public class HardMonster : Monster
{
    public override void Awake()
    {
        base.Awake();
        MonsterInfo = new MonsterInfo()
        {
            LivesCount = 3,
            StandardSpeed = 2,
            SpeedAttack = 3,
            CurrentSpeed = 2
        };
        ButtonCount = 4;
    }
}
