using UnityEngine;
using Game.Player;
using Game.Player.Components;
using Game.WeaponSystem.Enums;
using Game.WeaponSystem.Components;
using Game.WeaponSystem.Others;

namespace Game.WeaponSystem
{
    public partial class Weapon : MonoBehaviour
    {
        [Header("Settings")]
        public WeaponData data;
        public Transform fireRoot;
        public GameObject muzzleObject;

        [Header("Bullets")]
        public int bulletsPerMagazine = 12;
        public int currentBullets = 12;
        public int extraBullets = 24;

        public delegate void firing();
        public delegate void secureChanged();
        public delegate void startReload();
        public delegate void endReload();

        public firing onFiring;
        public secureChanged onSecureChanged;
        public startReload onStartReload;
        public endReload onEndReload;

        public bool isFiring { get; private set; }
        public bool isAiming { get; private set; }
        public bool isReloading { get; set; }
        public bool isDrawing { get; set; }
        public bool isHiding { get; set; }
        public bool isSafety { get; private set; }
        public bool haveBullets { get { return currentBullets > 0; } }
        public AudioClip overrideFireSound { get; set; }
        public WeaponAnimation weaponAnimation { get; private set; }

        private float drawTimer;
        private float hideTimer;
        private float firerateTimer;
        private float firingTimer;
        private Vector3 initialAimPos;
        private Quaternion initialAimRot;
        private Vector3 defaultAimPos;
        private Quaternion defaultAimRot;
        private Transform defaultFireRoot;
        private BulletDrop bulletDrop;

        public void ResetAim()
        {
            data.aimPosition = defaultAimPos;
            data.aimRotation = defaultAimRot;
        }

        public void ResetFireRoot()
        {
            fireRoot = defaultFireRoot;
        }

        private void Start()
        {
            // Default
            defaultFireRoot = fireRoot;
            defaultAimPos = data.aimPosition;
            defaultAimRot = data.aimRotation;

            // Aim
            initialAimPos = transform.localPosition;
            initialAimRot = transform.localRotation;

            // Delegates
            onFiring?.Invoke();
            onSecureChanged?.Invoke();

            // Get components
            weaponAnimation = GetComponentInChildren<WeaponAnimation>();
            bulletDrop = GetComponentInChildren<BulletDrop>();

            // Setting
            weaponAnimation.Init();
        }

        private void Update()
        {
            UpdateFire();
            UpdateReload();
            UpdateAim();
            UpdateMode();
        }

        private void UpdateFire()
        {
            bool conditionsToFire =
            (PlayerKeys.Click("Fire") || PlayerKeys.Press("Fire")) &&
            !PlayerController.isRunning && PlayerCamera.cursorLocked &&
             !isDrawing && !isHiding && !isReloading && !Retract.isRetracting;

            if (conditionsToFire)
            {
                switch (data.fireMode)
                {
                    case WeaponFireMode.Semi:

                        if (PlayerKeys.Click("Fire"))
                        {
                            firingTimer = data.firerate * 3f;
                            CalculateFire();
                        }

                        break;

                    case WeaponFireMode.Auto:

                        if (PlayerKeys.Press("Fire"))
                        {
                            firingTimer = data.firerate * 3f;
                            CalculateFire();
                        }

                        break;
                }
            }

            UpdateFirerate();
        }

        private void UpdateFirerate()
        {
            if (firingTimer >= 0)
            {
                isFiring = true;
                firingTimer -= Time.deltaTime;
            }
            else
            {
                isFiring = false;
            }

            if (firerateTimer >= 0)
            {
                firerateTimer -= Time.deltaTime;
            }
        }

        private void UpdateReload()
        {
            bool conditionsToReload =
            PlayerKeys.Click("Reload") &&
            (data.weaponMode == WeaponMode.Combat) &&
            !PlayerController.isRunning && PlayerCamera.cursorLocked &&
            !isReloading && !isDrawing && !isHiding && !Retract.isRetracting &&
            extraBullets > 0;

            if (conditionsToReload)
            {
                if (currentBullets <= 0)
                {
                    weaponAnimation.Play("Reload");
                }
                else if (currentBullets > 0 && currentBullets < bulletsPerMagazine)
                {
                    weaponAnimation.Play("Reload With Ammo");
                }
            }
        }

        private void UpdateAim()
        {
            bool conditionsToAim =
            PlayerKeys.Press("Aim") &&
            (data.weaponMode == WeaponMode.Combat) &&
            !PlayerController.isRunning && PlayerCamera.cursorLocked &&
            !isReloading && !isDrawing && !isHiding && !Retract.isRetracting;

            bool conditionsToReset =
            (transform.localPosition != initialAimPos || transform.localRotation != initialAimRot) &&
            !conditionsToAim;

            if (conditionsToAim)
            {
                isAiming = true;

                PlayerCamera.sensitivityScale = data.aimSensitivityScale;
                Sway.swayScale = data.aimSwayScale;
                Sway.swayHorizontalScale = 0f;

                transform.localPosition = Vector3.Lerp(transform.localPosition, data.aimPosition, data.aimSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, data.aimRotation, data.aimSpeed * Time.deltaTime);
            }
            else
            {
                PlayerCamera.sensitivityScale = 1;
                Sway.swayHorizontalScale = Sway.defaultSwayHorizontalScale;
                isAiming = false;
            }

            if (conditionsToReset)
            {
                Sway.swayScale = 1;
                transform.localPosition = Vector3.Lerp(transform.localPosition, initialAimPos, data.aimSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, initialAimRot, data.aimSpeed * Time.deltaTime);
            }
        }

        private void UpdateMode()
        {
            bool conditionsToChangeMode =
            PlayerKeys.Click("Safety") &&
            !isReloading && !isAiming && !isFiring && !isDrawing && !isHiding;

            if (conditionsToChangeMode)
            {
                switch (data.weaponMode)
                {
                    case WeaponMode.Secure: data.weaponMode = WeaponMode.Combat; break;
                    case WeaponMode.Combat: data.weaponMode = WeaponMode.Secure; break;
                }

                isSafety = data.weaponMode == WeaponMode.Secure;

                // Delegate
                onSecureChanged?.Invoke();
            }
        }
    }
}