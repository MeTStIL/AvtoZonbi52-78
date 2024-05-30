using System.Collections;
using DefaultNamespace;
using UnityEngine;

public interface IMonster
{
    Vector3 SpritePosition { get; set; }
    public int ButtonCount { get; set; }
    public MonsterInfo MonsterInfo { get; set; }
    void Awake();
    void LateUpdate();
    void Die();
    void GetDamage();
    void Walking();
    IEnumerator StopAndSetNewTarget();
    void SetNewTargetPosition();
}