using UnityEngine;

namespace WeaponSystem.Core
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "WeaponSystem/Create WeaponSO", order = 1)]
    public class WeaponSO : ScriptableObject
    {
        [Header("Fire")]
        public float firerate = 1f;
        public float muzzleTime = 0.05f;
        public float bulletHitForce = 200f;
        public float bulletVelocity = 200f;
        public float maxBulletDistance = 100f;
        public float effectiveDistance = 50f;
        public float bulletGravityScale = 2f;
        public int bulletsPerFire = 1;

        [Header("Aim")]
        public float aimSpeed = 5f;
        public Vector3 aimPosition;
        public Quaternion aimRotation;
        [Space]
        [Range(0f, 1f)] public float aimSenseScale = 1f;
        [Range(0f, 1f)] public float aimSwayScale = 0.2f;

        [Header("Recoil")]
        public Vector3 recoilForcePos;
        public Vector3 recoilForceRot;
        public Vector3 recoilForceCam;

        [Header("Damage")]
        public float minDamage = 20;
        public float maxDamage = 40;

        [Header("Audio")]
        public AudioClip[] customSounds;
        public AudioClip[] fireSounds;
        public AudioClip removingMagazineSound;
        public AudioClip putMagazineSound;
        public AudioClip cockingSound;
    }
}