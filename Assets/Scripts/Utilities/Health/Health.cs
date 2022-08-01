using UnityEngine;
using UnityEngine.Events;

namespace Game.Utilities
{
    public class Health : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] protected float currentHealth = 100f;
        [SerializeField] protected float maxHealth = 100f;

        [Header("Events")]
        [SerializeField] protected UnityEvent OnDamaged;
        [SerializeField] protected UnityEvent OnDeath;

        public void TakeDamage(float _damage)
        {
            float damage = _damage * -1;

            currentHealth -= damage;
            OnDamaged.Invoke();

            if (currentHealth < 0)
            {
                OnDeath.Invoke();
            }
        }
    }
}