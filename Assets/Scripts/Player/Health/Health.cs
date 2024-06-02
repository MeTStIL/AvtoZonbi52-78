using UnityEngine;

namespace Player.Health
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float startingHealth;
        public float CurrentHealth { get; private set; }

        public void Awake()
        {
            CurrentHealth = startingHealth;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, startingHealth);
            if (CurrentHealth == 0)
                Death.MoveToScreenDeath();
        }
    }
}
