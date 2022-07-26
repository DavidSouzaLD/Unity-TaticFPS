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
        if (!LockManager.IsLocked("PLAYER_ALL") && !LockManager.IsLocked("PLAYER_HABILITIES") && InputManager.Flashlight)
        {
            flashlightMode = !flashlightMode;
            lightObject.SetActive(flashlightMode);
        }
    }
}
