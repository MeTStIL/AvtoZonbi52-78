using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            if (currentHealth == 0)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void Update()
        {
        }
    }
}
