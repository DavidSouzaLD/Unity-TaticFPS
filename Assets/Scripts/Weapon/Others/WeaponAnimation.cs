using UnityEngine;

namespace Game.Weapon
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
        }

        private void LateUpdate()
        {
            // Active safe animation
            Animator.SetBool("Safety", Weapon.Preset.weaponMode == WeaponPreset.WeaponMode.Safety);
            Animator.SetBool("NoBullet", !Weapon.HaveBullets());
        }

        /// <summary>
        /// Sets the necessarys componenets.
        /// </summary>
        public void Init(Weapon _weapon, WeaponSound _sound)
        {
            Weapon = _weapon;
            Sound = _sound;
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
        }

        /// <summary>
        /// Calculates bullet loading.
        /// </summary>
        public void EndAnimation()
        {
            Weapon.SetState("Reloading", false);
        }
    }
}