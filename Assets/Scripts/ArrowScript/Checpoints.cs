using UnityEngine;
using System.Collections.Generic;
using Player;


public class CheckpointManager : MonoBehaviour
{
    public List<Transform> checkpoints; // Список всех чекпойнтов в сцене
    private int currentCheckpointIndex = 0; // Индекс текущего чекпойнта
    public float reachDistance = 1.0f; // Расстояние, при котором чекпойнт считается достигнутым
    public PlayerMovement player;
    

    private void Update()
    {
        // Проверяем, достиг ли игрок текущего чекпойнта
        if (checkpoints.Count > 0 && Vector3.Distance(player.transform.position, checkpoints[currentCheckpointIndex].position) < reachDistance)
        {
            // Переходим к следующему чекпойнту
            currentCheckpointIndex++;
            if (currentCheckpointIndex >= checkpoints.Count)
                currentCheckpointIndex = checkpoints.Count - 1;
        }
    }

    public Transform GetCurrentTarget()
    {
        if (checkpoints.Count > 0)
        {
            return checkpoints[currentCheckpointIndex];
        }
        else
        {
            return null;
        }
    }
}