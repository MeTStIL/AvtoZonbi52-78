using UnityEngine;
using Player;

public class RotateWithPlayer : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private void Update()
    {
        var target = playerMovement.checkpointManager.GetCurrentTarget();
        if (target == null) return;
        var direction = target.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}