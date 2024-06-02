using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenSystem : MonoBehaviour
{
    [SerializeField] private int neededTurnsCount;
    [SerializeField] private GameObject gear;
    private const float RotationSpeed = 1f;
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
        Console.WriteLine(2);
        gear.transform.Rotate(0, 0, RotationSpeed);
    }

    private void TryMoveToNextScene()
    {
        if (time >= 5f)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}