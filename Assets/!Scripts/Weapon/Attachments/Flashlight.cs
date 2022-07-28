using UnityEngine;

public class Flashlight : MonoBehaviour
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
        if (PlayerInput.WeaponHandguard)
        {
            flashlightMode = !flashlightMode;
            lightObject.SetActive(flashlightMode);
        }
    }
}
