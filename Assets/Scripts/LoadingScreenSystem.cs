using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenSystem : MonoBehaviour
{
    [SerializeField] private int neededTurnsCount;
    [SerializeField] private GameObject gear;
    private int turnsCount;
    private const float RotationSpeed = 0.5f;

    private void Update()
    {
        MakeGearTurn();
        TryMoveToNextScene();
    }

    private void MakeGearTurn()
    {
        gear.transform.Rotate(0, 0, RotationSpeed);
        if (Math.Abs(gear.transform.eulerAngles.z - 359) < 0.1)
            turnsCount++;
    }

    private void TryMoveToNextScene()
    {
        if (turnsCount == neededTurnsCount)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}