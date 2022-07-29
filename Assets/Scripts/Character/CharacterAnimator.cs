using UnityEngine;
using Game.Character;

namespace Game.Weapon
{
    public class CharacterAnimator : MonoBehaviour
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
            if (!WeaponManager.IsAim())
            {
                if (FPSCharacterController.GetState("GroundArea"))
                {
                    BasicAnimator.SetBool("Walking", FPSCharacterController.GetState("Walking"));
                    BasicAnimator.SetBool("Running", FPSCharacterController.GetState("Running"));
                }
            }
            else
            {
                BasicAnimator.SetBool("Walking", false);
                BasicAnimator.SetBool("Running", false);
            }

            // Aiming animation 
            BasicAnimator.SetBool("Aiming", WeaponManager.IsAim());

            // Air animation
            BasicAnimator.SetBool("Air", !FPSCharacterController.GetState("GroundArea"));
        }
    }
}