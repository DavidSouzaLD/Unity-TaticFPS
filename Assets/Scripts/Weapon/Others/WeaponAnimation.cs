using UnityEngine;

namespace Game.Weapons
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class WeaponAnimation : MonoBehaviour
    {
        // Private
        private Weapon Weapon;
        private WeaponSound Sound;
        private Animator Animator;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            Animator.keepAnimatorControllerStateOnDisable = true;
        }

        /// <summary>
        /// Sets the necessarys componenets.
        /// </summary>
        public void Init(Weapon _weapon, WeaponSound _sound)
        {
            // Setting components
            Weapon = _weapon;
            Sound = _sound;

            // Setting events
            Weapon.OnFiring += NoBulletCheck;
            Weapon.OnSafetyChanged += SafetyCheck;
            Weapon.OnEndReload += NoBulletCheck;
        }

        /// <summary>
        /// Checks if weapon safety has been changed to change animation.
        /// </summary>
        private void SafetyCheck()
        {
            Animator.SetBool("Safety", Weapon.Preset.weaponMode == WeaponPreset.WeaponMode.Safety);
        }

        /// <summary>
        /// Checks for bullet in magazine with each shot to trigger no bullet animation.
        /// </summary>
        private void NoBulletCheck()
        {
            Animator.SetBool("NoBullet", !Weapon.HaveBullets());
        }

        /// <summary>
        /// Play any animation in animator with animation name.
        /// </summary>
        public void Play(string _animationName)
        {
            Animator.Play(_animationName);
        }

        /// <summary>
        /// Starts when the animation starts.
        /// </summary>
        public void StartAnimation()
        {
            Weapon.SetState("Reloading", true);

            // Delegate
            Weapon.OnStartReload?.Invoke();
        }

        /// <summary>
        /// Activates the sound of removing the charger.
        /// </summary>
        public void RemoveMagazine()
        {
            Sound.Play("START_RELOAD");
        }

        /// <summary>
        /// Activates the sound of replacing the charger.
        /// </summary>
        public void PutMagazine()
        {
            Sound.Play("MIDDLE_RELOAD");
        }

        /// <summary>
        /// Activates the sound of cocking the weapon.
        /// </summary>
        public void Cocking()
        {
            Sound.Play("END_RELOAD");
            Weapon.CalculateReload();

            // Delegate
            Weapon.OnEndReload?.Invoke();
        }

        /// <summary>
        /// Calculates bullet loading.
        /// </summary>
        public void EndAnimation()
        {
            Weapon.SetState("Reloading", false);
        }

        /// <summary>
        /// Reset animator to a specific draw anim.
        /// </summary>
        public void ExitWeapon()
        {
            Animator.Play("Draw");
            Weapon.gameObject.SetActive(false);
        }
    }
}