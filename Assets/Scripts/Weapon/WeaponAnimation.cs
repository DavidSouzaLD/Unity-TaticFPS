using UnityEngine;

namespace Game.Weapon
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class WeaponAnimation : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            animator.keepAnimatorControllerStateOnDisable = true;
        }

        public void Init()
        {
            // Setting events
            WeaponManager.Instance.currentWeapon.onFiring += BulletsCheck;
            WeaponManager.Instance.currentWeapon.onSecureChanged += SafetyCheck;
            WeaponManager.Instance.currentWeapon.onEndReload += BulletsCheck;
        }

        public void Play(string animationName) => animator.Play(animationName);

        private void SafetyCheck()
        => animator.SetBool("Safety", WeaponManager.Instance.currentWeapon.data.weaponMode == WeaponMode.Secure);

        private void BulletsCheck()
        => animator.SetBool("NoBullet", !WeaponManager.Instance.currentWeapon.haveBullets);

        public void PlayEvent(WeaponEvents events)
        {
            switch (events)
            {
                case WeaponEvents.StartAnimation:

                    WeaponManager.Instance.currentWeapon.isReloading = true;
                    WeaponManager.Instance.currentWeapon.onStartReload?.Invoke();

                    break;

                case WeaponEvents.RemoveMagazine:

                    WeaponManager.Instance.PlaySound("REMOVING_MAGAZINE");

                    break;

                case WeaponEvents.PutMagazine:

                    WeaponManager.Instance.PlaySound("PUTTING_MAGAZINE");

                    break;

                case WeaponEvents.Cocking:

                    WeaponManager.Instance.PlaySound("COCKING");
                    WeaponManager.Instance.currentWeapon.CalculateReload();
                    WeaponManager.Instance.currentWeapon.onEndReload?.Invoke();

                    break;

                case WeaponEvents.EndAnimation:

                    WeaponManager.Instance.currentWeapon.isReloading = false;

                    break;
            }
        }

        public void PlayAudio(AudioClip clip)
        {
            WeaponManager.Instance.PlaySound("CUSTOM", 1f, clip);
        }
    }
}