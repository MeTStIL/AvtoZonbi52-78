using UnityEngine;
using UnityEngine.UI;

namespace Player.Health
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health playerHealth;
        [SerializeField] private Image totalHealthBar;
        [SerializeField] private Image currentHealthBar;

        private void Start()
        {
            totalHealthBar.fillAmount = playerHealth.CurrentHealth / 10;
        }

        private void Update()
        {
            currentHealthBar.fillAmount = playerHealth.CurrentHealth / 10;
        }
    }
}
