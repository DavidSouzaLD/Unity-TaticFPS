using UnityEngine;
using Game.Weapon;

namespace Game.Player
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
            if (WeaponManager.CurrentWeapon != null)
            {
                if (!WeaponManager.CurrentWeapon.isAiming)
                {
                    if (PlayerController.isGrounded)
                    {
                        BasicAnimator.SetBool("Aiming", false);
                        BasicAnimator.SetBool("Walking", PlayerController.isWalking);
                        BasicAnimator.SetBool("Running", PlayerController.isRunning);
                    }
                }
                else
                {
                    BasicAnimator.SetBool("Aiming", true);
                }
            }
            else
            {
                BasicAnimator.SetBool("Aiming", false);
                BasicAnimator.SetBool("Walking", false);
                BasicAnimator.SetBool("Running", false);
            }

            // Aiming animation 


            // Air animation
            BasicAnimator.SetBool("Air", !PlayerController.isGrounded);
        }
    }
}