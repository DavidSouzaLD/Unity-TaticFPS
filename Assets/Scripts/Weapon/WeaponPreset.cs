using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "WeaponPreset", menuName = "Game/Create WeaponPreset")]
    public class WeaponPreset : ScriptableObject
    {
        public enum FireMode { Semi, Auto }

        [Header("Settings")]
        public FireMode fireMode;
        public float firerate = 1f;

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
        public float maxBulletDistance;
        public float effectiveDistance;

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