using UnityEngine;

namespace Game.Weapon
{
    public class WeaponFlashlight : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameObject lightObject;
        private bool flashlightMode;

        private void Start()
        {
            lightObject.SetActive(flashlightMode);
        }

        private void Update()
        {
            // Night vision
            if (Systems.Input.GetBool("WeaponHandguard"))
            {
                flashlightMode = !flashlightMode;
                lightObject.SetActive(flashlightMode);
            }
        }
    }
}