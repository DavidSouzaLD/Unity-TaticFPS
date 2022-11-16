using UnityEngine;
using Game.Weapon;

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
            if (WeaponManager.Instance.currentWeapon != null)
            {
                BasicAnimator.SetBool("Aiming", WeaponManager.Instance.currentWeapon.isAiming);

                if (!WeaponManager.Instance.currentWeapon.isAiming)
                {
                    if (PlayerController.isGrounded)
                    {
                        BasicAnimator.SetBool("Walking", PlayerController.isWalking);
                        BasicAnimator.SetBool("Running", PlayerController.isRunning);
                    }

                    BasicAnimator.SetBool("Air", !PlayerController.isGrounded);
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