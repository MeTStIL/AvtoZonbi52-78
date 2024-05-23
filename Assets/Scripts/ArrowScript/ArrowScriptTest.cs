using UnityEngine;
using Player;

public class RotateWithPlayer : MonoBehaviour
{
    public PlayerMovement playerMovement; // Добавьте это поле в инспектор и прикрепите PlayerMovement к стрелке

    void Update()
    {
        Transform target = playerMovement.checkpointManager.GetCurrentTarget();
        if (target != null)
        {
            // Вычисляем угол поворота игрока
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Поворачиваем объект
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}