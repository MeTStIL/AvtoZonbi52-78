using System;
using UnityEngine;

namespace Player.Health
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float startingHealth;
        public float currentHealth { get; private set; }

        public void Awake()
        {
            currentHealth = startingHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

            if (currentHealth > 0)
            {
                
            }
            else
            {
                
            }
        }

        private void Update()
        {
        }
    }
}
