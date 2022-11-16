using UnityEngine;
using Game.WeaponSystem.Enums;

namespace Game.WeaponSystem
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class WeaponAnimation : MonoBehaviour
    {
        private Weapon parent;
        private Animator animator;
        private bool noBullets;

        private void Awake()
        {
            parent = GetComponentInParent<Weapon>();
            animator = GetComponent<Animator>();

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

        public void Play(string animationName, bool force = false)
        {
            if (!force)
            {
                bool conditionsToPlay = !parent.isDrawing && !parent.isHiding;

                if (conditionsToPlay)
                {
                    animator.Play(animationName);
                }
            }
            else
            {
                animator.Play(animationName);
            }
        }

        private void SafetyCheck()
        {
            animator.SetBool("Safety", parent.data.weaponMode == WeaponMode.Secure);
        }

        private void BulletsCheck()
        {
            if (!parent.isReloading)
            {
                noBullets = !parent.haveBullets;
                animator.SetBool("NoBullet", noBullets);
            }
            else
            {
                animator.SetBool("NoBullet", false);
            }
        }

        public void SetDrawing(int value)
        {
            parent.isDrawing = value > 0;
        }

        public void SetHiding(int value)
        {
            parent.isHiding = value > 0;
        }

        public void PlayEvent(WeaponEvents events)
        {
            switch (events)
            {
                case WeaponEvents.StartAnimation:

                    parent.isReloading = true;
                    parent.onStartReload?.Invoke();

                    // Reset noBullet
                    BulletsCheck();

                    break;

                case WeaponEvents.RemoveMagazine:

                    WeaponManager.PlaySound("REMOVING_MAGAZINE");

                    break;

                case WeaponEvents.PutMagazine:

                    WeaponManager.PlaySound("PUTTING_MAGAZINE");

                    break;

                case WeaponEvents.Cocking:

                    WeaponManager.PlaySound("COCKING");

                    break;

                case WeaponEvents.EndAnimation:

                    parent.isReloading = false;
                    parent.CalculateReload();
                    parent.onEndReload?.Invoke();

                    break;
            }
        }

        public void PlayAudio(AudioClip clip)
        {
            WeaponManager.PlaySound("CUSTOM", 1f, clip);
        }
    }
}