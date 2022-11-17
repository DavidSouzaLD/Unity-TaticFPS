using UnityEngine;

namespace WeaponSystem.Core
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "WeaponSystem/Create WeaponSO", order = 1)]
    public class WeaponSO : ScriptableObject
    {
        [Header("Fire")]
        public float firerate;
        public float bulletHitForce;
        public float bulletVelocity;
        public float maxBulletDistance;
        public float effectiveDistance;
        public float bulletGravityScale;
        public int bulletsPerFire;

        [Header("Magazine")]
        public int startMagazinesQuantity;
        public int bulletsPerMagazine;

        [Header("Damage")]
        public float minDamage;
        public float maxDamage;

        [Header("Audio")]
        public AudioClip[] customSounds;
        public AudioClip[] fireSounds;
        public AudioClip removingMagazineSound;
        public AudioClip putMagazineSound;
        public AudioClip cockingSound;
    }
}