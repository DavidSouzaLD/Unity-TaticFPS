using UnityEngine;
using WeaponSystem.Core;

namespace Game.Player.Components
{
    public class PlayerLean : MonoBehaviour
    {
        [Header("Lean")]
        public float leanAmount = 20f;
        public float leanCamScale = 0.02f;
        public float leanSpeed = 6f;

        [Header("Roots")]
        [SerializeField] private Transform leanWeaponRoot;
        [SerializeField] private Transform leanCamRoot;

        private Vector3 initialCoverCamPos;
        private Quaternion initialCoverRot;
        private Quaternion initialCoverCamRot;

        private void Start()
        {
            // Cover
            initialCoverRot = leanWeaponRoot.localRotation;
            initialCoverCamPos = leanCamRoot.localPosition;
            initialCoverCamRot = leanCamRoot.localRotation;
        }

        private void Update()
        {
            float leanInput = InputManager.GetFloat("Lean");
            bool conditions = leanInput != 0;

            if (conditions)
            {
                Vector3 targetPos = new Vector3(leanAmount * leanCamScale * leanInput, 0f, 0f);
                Quaternion targetRot = Quaternion.Euler(new Vector3(0f, 0f, leanAmount * -leanInput));

                bool conditionsToLeanWeapon = WeaponManager.CurrentWeapon != null && !WeaponManager.CurrentWeapon.isAiming;

                if (conditionsToLeanWeapon)
                {
                    // Weapon
                    leanWeaponRoot.localRotation = Quaternion.Slerp(leanWeaponRoot.localRotation, targetRot, leanSpeed * Time.deltaTime);
                    leanCamRoot.localRotation = Quaternion.Slerp(leanCamRoot.localRotation, initialCoverCamRot, leanSpeed * Time.deltaTime);
                    leanCamRoot.localPosition = Vector3.Lerp(leanCamRoot.localPosition, initialCoverCamPos, leanSpeed * Time.deltaTime);
                }
                else
                {
                    // Camera
                    leanWeaponRoot.localRotation = Quaternion.Slerp(leanWeaponRoot.localRotation, initialCoverRot, leanSpeed * Time.deltaTime);
                    leanCamRoot.localRotation = Quaternion.Slerp(leanCamRoot.localRotation, targetRot, leanSpeed * Time.deltaTime);
                    leanCamRoot.localPosition = Vector3.Lerp(leanCamRoot.localPosition, targetPos, leanSpeed * Time.deltaTime);
                }
            }
            else if (leanWeaponRoot.localRotation != initialCoverRot || leanCamRoot.localRotation != initialCoverCamRot || leanCamRoot.localPosition != initialCoverCamPos)
            {
                leanWeaponRoot.localRotation = Quaternion.Slerp(leanWeaponRoot.localRotation, initialCoverRot, leanSpeed * Time.deltaTime);
                leanCamRoot.localRotation = Quaternion.Slerp(leanCamRoot.localRotation, initialCoverCamRot, leanSpeed * Time.deltaTime);
                leanCamRoot.localPosition = Vector3.Lerp(leanCamRoot.localPosition, initialCoverCamPos, leanSpeed * Time.deltaTime);
            }
        }
    }
}