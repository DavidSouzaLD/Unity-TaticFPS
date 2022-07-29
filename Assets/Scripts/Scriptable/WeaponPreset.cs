using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "WeaponPreset", menuName = "Game/Create WeaponPreset")]
    public class WeaponPreset : ScriptableObject
    {
        public enum WeaponMode { Safety, Combat }
        public enum FireMode { Semi, Auto }

        [Header("Settings")]
        public WeaponMode weaponMode;
        public FireMode fireMode;
        public float firerate = 1f;

        [Header("Switch")]
        public float drawTime;
        public float hideTime;

        [Header("Bullet")]
        public float bulletHitForce = 100f;
        public float bulletVelocity = 25f;
        public int bulletsPerFire = 1;
        public float bulletGravityScale = 1;
        public float maxBulletDistance;

        [Header("Aim")]
        public float aimSpeed = 5f;
        public float aimSwayScale = 0.2f;
        public Vector3 aimPosition;
        public Quaternion aimRotation;

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