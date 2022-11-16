using UnityEngine;
using Game.Weapon.Enums;

namespace Game.Weapon
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class WeaponAnimation : MonoBehaviour
    {
        private Weapon parent;
        private Animator animator;

        private void Awake()
        {
            parent = GetComponentInParent<Weapon>();
            animator = GetComponent<Animator>();
            animator.keepAnimatorControllerStateOnDisable = true;

            if (parent == null)
            {
                Debug.LogError("(WeaponAnimation) Parent not found!");
            }
        }

        public void Init()
        {
            // Setting events
            parent.onFiring += BulletsCheck;
            parent.onSecureChanged += SafetyCheck;
            parent.onEndReload += BulletsCheck;
        }

        public void Play(string animationName) => animator.Play(animationName);

        private void SafetyCheck()
        => animator.SetBool("Safety", parent.data.weaponMode == WeaponMode.Secure);

        private void BulletsCheck()
        => animator.SetBool("NoBullet", !parent.haveBullets);

        public void PlayEvent(WeaponEvents events)
        {
            switch (events)
            {
                case WeaponEvents.StartAnimation:

                    parent.isReloading = true;
                    parent.onStartReload?.Invoke();

                    break;

                case WeaponEvents.RemoveMagazine:

                    WeaponManager.Instance.PlaySound("REMOVING_MAGAZINE");

                    break;

                case WeaponEvents.PutMagazine:

                    WeaponManager.Instance.PlaySound("PUTTING_MAGAZINE");

                    break;

                case WeaponEvents.Cocking:

                    WeaponManager.Instance.PlaySound("COCKING");

                    break;

                case WeaponEvents.EndAnimation:

                    parent.isReloading = false;
                    parent.CalculateReload();
                    parent.onEndReload?.Invoke();
                    Debug.Log("OJ");

                    break;
            }
        }

        public void PlayAudio(AudioClip clip)
        {
            WeaponManager.Instance.PlaySound("CUSTOM", 1f, clip);
        }
    }
}