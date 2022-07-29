using UnityEngine;

namespace Game.Weapon
{
    public class WeaponHitmark : Singleton<WeaponHitmark>
    {
        [Header("HitMark")]
        [SerializeField] private GameObject hitMark;
        [SerializeField] private AudioClip hitMarkSound;
        [SerializeField] private float hitMarkTime;

        // Private
        private float timerHitMark;

        public static AudioClip GetHitMarkSound()
        {
            return Instance.hitMarkSound;
        }

        private void Start()
        {
            // Others
            hitMark.SetActive(false);
        }

        private void Update()
        {
            if (timerHitMark > 0)
            {
                if (timerHitMark - Time.deltaTime <= 0)
                {
                    hitMark.SetActive(false);
                }

                timerHitMark -= Time.deltaTime;
            }
        }

        public static void ApplyHitMark(Vector3 _position)
        {
            if (Instance.timerHitMark <= 0)
            {
                Instance.hitMark.transform.position = _position;
                Instance.hitMark.SetActive(true);
                Instance.timerHitMark = Instance.hitMarkTime;
            }
        }
    }
}