using System.Collections.Generic;
using Player;
using UnityEngine;

namespace ArrowScript
{
    public class CheckpointManager : MonoBehaviour
    {
        public List<Transform> checkpoints;
        private int currentCheckpointIndex;
        public float reachDistance = 1.0f;
        public PlayerMovement player;
        
        private void Update()
        {
            if (checkpoints.Count <= 0 || !(Vector3.Distance(player.transform.position, 
                    checkpoints[currentCheckpointIndex].position) < reachDistance)) 
                return;
            currentCheckpointIndex++;
            if (currentCheckpointIndex >= checkpoints.Count)
                currentCheckpointIndex = checkpoints.Count - 1;
        }

        public Transform GetCurrentTarget()
        {
            return checkpoints.Count > 0 ? checkpoints[currentCheckpointIndex] : null;
        }
    }
}