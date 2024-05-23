using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D body;
        [SerializeField] public float speed;
        [SerializeField] private Animator animator;
        private Vector2 direction;
        public GameObject arrow;
        public CheckpointManager checkpointManager; // Добавьте это поле в инспектор и прикрепите CheckpointManager к игроку

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

            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("speed", direction.sqrMagnitude);

            if (Input.GetMouseButtonDown(1)) // Проверяем, была ли нажата правая кнопка мыши
            {
                arrow.SetActive(!arrow.activeSelf); // Переключаем видимость стрелки
            }
        }

        private void FixedUpdate()
        {
            body.MovePosition(body.position + direction * speed * Time.fixedDeltaTime);
        }
    }
}