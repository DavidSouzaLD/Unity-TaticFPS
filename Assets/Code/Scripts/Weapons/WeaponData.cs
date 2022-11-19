using UnityEngine;

namespace Code.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon/WeaponData", order = 1)]
    public class WeaponData : ScriptableObject
    {
        [Header("Settings")]
        public Weapon.Type type;
        public Weapon.FireMode fireMode;

        [Header("Fire")]
        public float firerate;
        public float burstRate;
        public int burstPerFire;
        public int bulletsPerFire = 1;
        public float maxBulletDistance;
        public float effectiveBulletDistance;
        public float bulletGravityScale;
        public float bulletVelocity;
        public float bulletHitForce;

        [Header("Magazine")]
        public int initialMagazines;
        public int bulletsPerMagazine;

        [Header("Aim")]
        public AnimationCurve aimCurve;
        public float aimSpeed = 5f;
        public Vector3 aimPosition;
        public Quaternion aimRotation;
        [Space]
        [Range(0f, 1f)] public float aimSensitivityScale = 1f;
        [Range(0f, 1f)] public float aimSwayScale = 0.2f;

        [Header("Recoil")]
        public Vector3 recoilForcePos;
        public Vector3 recoilForceRot;
        public Vector3 recoilForceCam;

        [Header("Damage")]
        public int minDamage;
        public int maxDamage;

        [Header("Audio")]
        public AudioClip[] fireSounds;
        public AudioClip removingMagazineSound;
        public AudioClip putMagazineSound;
        public AudioClip cockingSound;
    }
}