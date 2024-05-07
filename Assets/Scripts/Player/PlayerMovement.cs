using System;
using UnityEngine;

namespace Player
{
   public class PlayerMovement : MonoBehaviour
   {
      private Rigidbody2D body;
      [SerializeField] public float speed;
      [SerializeField]private Animator animator;
      private Vector2 direction;

      // Каждый раз когда запускаем игру, этот метод выполняется
      private void Awake()
      {
         body = GetComponent<Rigidbody2D>();
      }

      private void Update()
      {
         direction.x = Input.GetAxis("Horizontal");
         direction.y = Input.GetAxis("Vertical");
         
         animator.SetFloat("Horizontal", direction.x);
         animator.SetFloat("Vertical", direction.y);
         animator.SetFloat("speed", direction.sqrMagnitude); // direction.magnitude - длина вектора
      }

      private void FixedUpdate()
      {
         //Time.fixedDeltaTime - время с последнего обновления данной функции
         body.MovePosition(body.position + direction * speed * Time.fixedDeltaTime);
      }
   }
}
