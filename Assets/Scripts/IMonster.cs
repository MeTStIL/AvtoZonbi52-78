using System.Collections;
using UnityEngine;

public interface IMonster
{
    int LivesCount { get; set; }
    float StandardSpeed { get; set; }
    Vector3 SpritePosition { get; set; }
    public int ButtonCount { get; set; }
    void Awake();
    void LateUpdate();
    void Die();
    void GetDamage();
    void Walking();
    IEnumerator StopAndSetNewTarget();
    void SetNewTargetPosition();
}