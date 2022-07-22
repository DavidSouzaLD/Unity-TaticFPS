using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipment : MonoBehaviour
{
    [Header("[Attachments UI]")]
    public bool customMode;
    [Space]
    public Transform muzzlePosition;
    public Image muzzleButton;
    [Space]
    public Transform handguardPosition;
    public Image handguardButton;

    bool changeMode;

    public WeaponSystem firstWeapon, secondWeapon;

    private void LateUpdate()
    {
        customMode = PlayerInput.Keys.Custom;

        if (customMode)
        {
            if (!changeMode)
            {
                muzzleButton.enabled = true;
                handguardButton.enabled = true;
                changeMode = true;
            }

            muzzleButton.rectTransform.position = PlayerCamera.WorldToScreen(muzzlePosition.position);
            handguardButton.rectTransform.position = PlayerCamera.WorldToScreen(handguardPosition.position);
        }
        else
        {
            if (changeMode)
            {
                muzzleButton.enabled = false;
                handguardButton.enabled = false;
                changeMode = false;
            }
        }
    }

    public void ChangeWeapon(int _id)
    {
        int selectedWeapon = (int)Mathf.Clamp(_id, 1, 2);

        switch (selectedWeapon)
        {
            case 1:
                firstWeapon.gameObject.SetActive(true);
                secondWeapon.gameObject.SetActive(false);
                break;

            case 2:
                firstWeapon.gameObject.SetActive(false);
                secondWeapon.gameObject.SetActive(true);
                break;
        }
    }
}
