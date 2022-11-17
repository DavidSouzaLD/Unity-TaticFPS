using UnityEngine;
using WeaponSystem.Core;

namespace Game.Player.Components
{
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Animator BasicAnimator;

        private void LateUpdate()
        {
            BasicUpdate();
        }

        private void BasicUpdate()
        {
            // Moving animation
            if (WeaponSettings.CurrentWeapon != null)
            {
                BasicAnimator.SetBool("Aiming", WeaponSettings.CurrentWeapon.isAiming);

                if (!WeaponSettings.CurrentWeapon.isAiming)
                {
                    if (PlayerController.isGrounded)
                    {
                        BasicAnimator.SetBool("Walking", PlayerController.isWalking);
                        BasicAnimator.SetBool("Running", PlayerController.isRunning);
                    }

                    BasicAnimator.SetBool("Air", !PlayerController.isGrounded);
                }
                else
                {
                    BasicAnimator.SetBool("Air", false);
                }
            }
            else if (BasicAnimator.GetBool("Aiming") || BasicAnimator.GetBool("Walking") ||
                     BasicAnimator.GetBool("Running") || BasicAnimator.GetBool("Air"))
            {
                BasicAnimator.SetBool("Aiming", WeaponSettings.CurrentWeapon.isAiming);
                BasicAnimator.SetBool("Walking", false);
                BasicAnimator.SetBool("Running", false);
                BasicAnimator.SetBool("Air", false);
            }
        }
    }
}