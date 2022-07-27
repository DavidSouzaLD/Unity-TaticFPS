using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [Header("[Flashlight Settings]")]
    [SerializeField] private bool flashlightMode;
    [SerializeField] private GameObject lightObject;

    private void Start()
    {
        lightObject.SetActive(flashlightMode);
    }

    private void Update()
    {
        // Night vision
        if (InputManager.WeaponHandguard)
        {
            flashlightMode = !flashlightMode;
            lightObject.SetActive(flashlightMode);
        }
    }
}