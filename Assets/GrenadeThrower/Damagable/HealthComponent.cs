using UnityEngine;

namespace GrenadeThrower.Damagable
{
    public class HealthComponent : MonoBehaviour, IDamagable
    {
        [Min(1)]
        [SerializeField]
        private int maxHealth = 100;

        [SerializeField]
        private bool respawn;
        
        [SerializeField]
        private float respawnDelay;
        
        private int _currentHealth;
        
        private void Awake()
        {
            ResetHealth();
        }

        public void Damage(int damage)
        {
            _currentHealth -= damage;
            DamageIndicator.Display(transform.position, damage);
            if (_currentHealth <= 0)
            {
                KillSelf();
            }
        }

        private void ResetHealth()
        {
            _currentHealth = maxHealth;
        }

        private void KillSelf()
        {
            if (respawn)
            {
                gameObject.SetActive(false);
                Invoke(nameof(Reactivate), respawnDelay);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Reactivate()
        {
            ResetHealth();
            gameObject.SetActive(true);
        }
    }
}
