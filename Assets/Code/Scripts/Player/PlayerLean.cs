using UnityEngine;
using Code.Systems.InputSystem;
using Code.Interfaces;

namespace Code.Player
{
    public class PlayerLean : MonoBehaviour, IPlayerControllerComponent
    {
        [Header("Settings")]
        [SerializeField] private Transform leanWeaponRoot;
        [SerializeField] private Transform leanCamRoot;

        public PlayerController playerController { get; set; }
        public bool isLeaning { get; set; }

        private Vector3 initialLeanCamPos;
        private Quaternion initialLeanRot;
        private Quaternion initialLeanCamRot;

        public void SetPlayerController(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        private void Start()
        {
            InitVariables();
        }

        private void InitVariables()
        {
            initialLeanRot = leanWeaponRoot.localRotation;
            initialLeanCamPos = leanCamRoot.localPosition;
            initialLeanCamRot = leanCamRoot.localRotation;
        }

        private void Update()
        {
            float leanInput = InputSystem.GetFloat("Lean");
            isLeaning = InputSystem.GetFloat("Lean") != 0;

            if (PlayerController.isLeaning)
            {
                Vector3 targetPos = new Vector3(playerController.data.leanAmount * playerController.data.leanCamScale * leanInput, 0f, 0f);
                Quaternion targetRot = Quaternion.Euler(new Vector3(0f, 0f, playerController.data.leanAmount * -leanInput));

                bool conditions = false; //WeaponManager.CurrentWeapon != null && !WeaponManager.CurrentWeapon.isAiming;

                if (conditions)
                {
                    // Weapon
                    leanWeaponRoot.localRotation = Quaternion.Slerp(leanWeaponRoot.localRotation, targetRot, playerController.data.leanSpeed * Time.deltaTime);
                    leanCamRoot.localRotation = Quaternion.Slerp(leanCamRoot.localRotation, initialLeanCamRot, playerController.data.leanSpeed * Time.deltaTime);
                    leanCamRoot.localPosition = Vector3.Lerp(leanCamRoot.localPosition, initialLeanCamPos, playerController.data.leanSpeed * Time.deltaTime);
                }
                else
                {
                    // Camera
                    leanWeaponRoot.localRotation = Quaternion.Slerp(leanWeaponRoot.localRotation, initialLeanRot, playerController.data.leanSpeed * Time.deltaTime);
                    leanCamRoot.localRotation = Quaternion.Slerp(leanCamRoot.localRotation, targetRot, playerController.data.leanSpeed * Time.deltaTime);
                    leanCamRoot.localPosition = Vector3.Lerp(leanCamRoot.localPosition, targetPos, playerController.data.leanSpeed * Time.deltaTime);
                }
            }
            else if (leanWeaponRoot.localRotation != initialLeanRot || leanCamRoot.localRotation != initialLeanCamRot || leanCamRoot.localPosition != initialLeanCamPos)
            {
                leanWeaponRoot.localRotation = Quaternion.Slerp(leanWeaponRoot.localRotation, initialLeanRot, playerController.data.leanSpeed * Time.deltaTime);
                leanCamRoot.localRotation = Quaternion.Slerp(leanCamRoot.localRotation, initialLeanCamRot, playerController.data.leanSpeed * Time.deltaTime);
                leanCamRoot.localPosition = Vector3.Lerp(leanCamRoot.localPosition, initialLeanCamPos, playerController.data.leanSpeed * Time.deltaTime);
            }
        }
    }
}