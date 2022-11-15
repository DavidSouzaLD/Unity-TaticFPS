using UnityEngine;

namespace Game.Weapon
{

    [CreateAssetMenu(fileName = "WeaponData", menuName = "PPM22/WeaponData", order = 0)]
    public class WeaponData : ScriptableObject
    {
        [Header("Settings")]
        public WeaponMode weaponMode;
        public WeaponFireMode fireMode;
        public float firerate = 1f;
        public float muzzleTime = 0.01f;

        [Header("Aim")]
        public float aimSpeed = 5f;
        public Vector3 aimPosition;
        public Quaternion aimRotation;
        [Space]
        [Range(0f, 1f)] public float aimSensitivityScale = 1f;
        [Range(0f, 1f)] public float aimSwayScale = 0.2f;

        [Header("Damage")]
        public float minDamage = 35f;
        public float maxDamage = 60f;

        [Header("Switch")]
        public float drawTime;
        public float hideTime;

        [Header("Bullet")]
        public float bulletHitForce = 100f;
        public float bulletVelocity = 25f;
        public int bulletsPerFire = 1;
        public float bulletGravityScale = 1;
        public float maxBulletDistance = 100f;
        public float effectiveDistance = 70f;

        [Header("Recoil")]
        public Vector3 recoilForcePos;
        public Vector3 recoilForceRot;
        public Vector3 recoilForceCam;

        [Header("Sounds")]
        public AudioClip[] fireSounds;
        public AudioClip noBulletSound;
        public AudioClip startReloadSound;
        public AudioClip middleReloadSound;
        public AudioClip endReloadSound;
    }
}