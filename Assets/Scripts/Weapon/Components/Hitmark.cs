using UnityEngine;

namespace Game.Weapon
{
    public class Hitmark : Singleton<Hitmark>
    {
        [Header("HitMark")]
        public GameObject hitMark;
        public AudioClip hitMarkSound;
        public float hitMarkTime;
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