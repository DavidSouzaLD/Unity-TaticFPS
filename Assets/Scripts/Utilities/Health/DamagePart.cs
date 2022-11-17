using UnityEngine;

namespace Game.Utilities
{
    public class DamagePart : MonoBehaviour, IDamageable<float>
    {
        [Header("Settings")]
        [SerializeField] private Health health;
        [SerializeField] private float currentHealth = 100f;
        [SerializeField, Range(0f, 10f)] private float multiplierDamage = 1f;

        public void TakeDamage(float _damage)
        {
            health.TakeDamage(-_damage * multiplierDamage);
        }
    }
}