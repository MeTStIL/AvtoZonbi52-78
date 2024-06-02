using UnityEngine;

public class DeathScreenSystem : MonoBehaviour
{
    [SerializeField] private int neededTurnsCount;
    [SerializeField] private GameObject gear;
    private int turnsCount;
    private const float RotationSpeed = 0.5f;
    private float time;
    
    private void Start()
    {
        time = 0;
    }
    
    private void Update()
    {
        time += Time.deltaTime;
        MakeGearTurn();
        TryMoveToNextScene();
    }

    private void MakeGearTurn()
    {
        gear.transform.Rotate(0, 0, RotationSpeed);
        if (time >= 5f)
            turnsCount++;
    }

    private void TryMoveToNextScene()
    {
        if (turnsCount == neededTurnsCount)
            Death.Respawn();
    }
}
