using UnityEngine;

namespace Player
{
   public class PlayerMovement : MonoBehaviour
   {
      private Rigidbody2D body;
      [SerializeField] private float speed;

      // Каждый раз когда запускаем игру, этот метод выполняется
      private void Awake()
      {
         body = GetComponent<Rigidbody2D>();
      }

      private void Update()
      {
         body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
      
      }
   }
}
