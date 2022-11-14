using UnityEngine;

namespace Game.Weapon
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(WeaponSound))]
    public class WeaponAnimation : MonoBehaviour
    {
        // Private
        private Weapon weapon;
        private WeaponSound weaponSound;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            weaponSound = GetComponent<WeaponSound>();

            animator.keepAnimatorControllerStateOnDisable = true;
        }

        /// <summary>
        /// Sets the necessarys componenets.
        /// </summary>
        public void Init(Weapon sendWeapon, WeaponSound sendWeaponSound)
        {
            // Setting components
            weapon = sendWeapon;
            weaponSound = sendWeaponSound;

            // Setting events
            weapon.onFiring += NoBulletCheck;
            weapon.onSecureChanged += SafetyCheck;
            weapon.onEndReload += NoBulletCheck;
        }

        /// <summary>
        /// Checks if weapon safety has been changed to change animation.
        /// </summary>
        private void SafetyCheck()
        {
            animator.SetBool("Safety", weapon.weaponMode == WeaponMode.Secure);
        }

        /// <summary>
        /// Checks for bullet in magazine with each shot to trigger no bullet animation.
        /// </summary>
        private void NoBulletCheck()
        {
            animator.SetBool("NoBullet", !weapon.haveBullets);
        }

        /// <summary>
        /// Play any animation in animator with animation name.
        /// </summary>
        public void Play(string animationName)
        {
            animator.Play(animationName);
        }

        /// <summary>
        /// Starts when the animation starts.
        /// </summary>
        public void StartAnimation()
        {
            weapon.isReloading = true;

            // Delegate
            weapon.onStartReload?.Invoke();
        }

        /// <summary>
        /// Activates the sound of removing the charger.
        /// </summary>
        public void RemoveMagazine()
        {
            weaponSound.Play("START_RELOAD");
        }

        /// <summary>
        /// Activates the sound of replacing the charger.
        /// </summary>
        public void PutMagazine()
        {
            weaponSound.Play("MIDDLE_RELOAD");
        }

        /// <summary>
        /// Activates the sound of cocking the weapon.
        /// </summary>
        public void Cocking()
        {
            weaponSound.Play("END_RELOAD");
            weapon.CalculateReload();

            // Delegate
            weapon.onEndReload?.Invoke();
        }

        /// <summary>
        /// Calculates bullet loading.
        /// </summary>
        public void EndAnimation()
        {
            weapon.isReloading = false;
        }

        /// <summary>
        /// Reset animator to a specific draw anim.
        /// </summary>
        public void HideWeapon()
        {
            animator.Play("Draw");
            weapon.gameObject.SetActive(false);
        }
    }
}