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
            if (WeaponManager.Instance.currentWeapon != null)
            {
                if (!WeaponManager.Instance.currentWeapon.isAiming)
                {
                    if (PlayerController.isGrounded)
                    {
                        BasicAnimator.SetBool("Aiming", false);
                        BasicAnimator.SetBool("Walking", PlayerController.isWalking);
                        BasicAnimator.SetBool("Running", PlayerController.isRunning);
                    }

                    BasicAnimator.SetBool("Air", !PlayerController.isGrounded);
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
        }
    }
}