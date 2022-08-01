using UnityEngine;
using Game.Utilities;

namespace Game.Training
{
    public class Target : Health
    {
        [Header("Other")]
        [SerializeField] private float timeToRevive = 5f;
        [SerializeField] private Animator Animator;

        private void Start()
        {
            OnDeath.AddListener(DeathAnimation);
        }

        public void ReviveAnimation()
        {
            Animator.SetBool("Dead", false);
            currentHealth = maxHealth;
        }

        public void DeathAnimation()
        {
            Animator.SetBool("Dead", true);
            Invoke("ReviveAnimation", timeToRevive);
        }
    }
}