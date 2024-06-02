using ArrowScript;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : Sounds
    {
        [SerializeField] public float speed;
        [SerializeField] private Animator animator;
        private Vector2 direction;
        private Rigidbody2D body;
        public GameObject arrow;
        public CheckpointManager checkpointManager;
        private bool isMoving;
        private bool isPaused;
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Speed = Animator.StringToHash("speed");

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            arrow.SetActive(false);
        }

        private void Update()
        {
            arrow.transform.position = body.transform.position;
            direction.x = Input.GetAxis("Horizontal");
            direction.y = Input.GetAxis("Vertical");
            animator.SetFloat(Horizontal, direction.x);
            animator.SetFloat(Vertical, direction.y);
            animator.SetFloat(Speed, direction.sqrMagnitude);

            if (Input.GetMouseButtonDown(1))
            {
                arrow.SetActive(!arrow.activeSelf);
            }
            isPaused = PauseMenu.IsPaused;
            if (!isPaused)
            {
                ResumeMusic();
                switch (direction.sqrMagnitude)
                {
                    case > 0 when !isMoving:
                        isMoving = true;
                        PlaySound(objectSounds[0], volume: 0.8f);
                        break;
                    case 0 when isMoving:
                        isMoving = false;
                        StopSound();
                        break;
                }
            }
            else
                PauseMusic();
        }

        private void FixedUpdate()
        {
            body.MovePosition(body.position + direction * (speed * Time.fixedDeltaTime));
        }
    }
}