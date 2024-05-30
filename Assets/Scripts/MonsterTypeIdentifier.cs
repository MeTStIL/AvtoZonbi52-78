using UnityEngine;

public class MonsterTypeIdentifier : MonoBehaviour
{
    public Monster other;
        
    public void Awake()
    {
        if (gameObject.TryGetComponent(out MediumMonster mediumMonster))
            other = mediumMonster;
        else if (gameObject.TryGetComponent(out SmallMonster smallMonster))
            other = smallMonster;
        else if (gameObject.TryGetComponent(out HardMonster hardMonster))
            other = hardMonster;
    }
}