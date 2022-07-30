using UnityEngine;

namespace Game.Weapons.Attachments
{
    public class Flashlight : Attachment
    {
        [Header("Settings")]
        [SerializeField] private GameObject lightObject;

        protected bool isEnabled;
        protected bool changedMode;

        private void Start()
        {
            lightObject.SetActive(isEnabled);
        }

        private void Update()
        {
            // Night vision
            if (Systems.Input.GetBool("WeaponHandguard"))
            {
                isEnabled = !isEnabled;
                lightObject.SetActive(isEnabled);
            }
        }
    }
}