using UnityEngine;

public class WeaponEvents : MonoBehaviour
{
    protected Weapon Weapon;

    private void OnDisable()
    {
        Weapon.GetAnimator().Play("Draw", -1, 0f);
    }

    private void LateUpdate()
    {
        // Active safe animation
        if (Weapon != null)
        {
            Weapon.GetAnimator().SetBool("Safety", Weapon.Preset.weaponMode == WeaponPreset.WeaponMode.Safety);
        }
    }

    /// <summary>
    /// Sets the event weapon.
    /// </summary>
    public void SetWeapon(Weapon weapon)
    {
        Weapon = weapon;
    }

    /// <summary>
    /// Starts when the animation starts.
    /// </summary>
    public void StartAnimation()
    {
        Weapon.GetStates.SetState("Reloading", true);
    }

    /// <summary>
    /// Activates the sound of removing the charger.
    /// </summary>
    public void RemoveMagazine()
    {
        Weapon.PlaySound("START_RELOAD");
    }

    /// <summary>
    /// Activates the sound of replacing the charger.
    /// </summary>
    public void PutMagazine()
    {
        Weapon.PlaySound("MIDDLE_RELOAD");
    }

    /// <summary>
    /// Activates the sound of cocking the weapon.
    /// </summary>
    public void Cocking()
    {
        Weapon.GetAnimator().SetBool("NoBullet", false);
        Weapon.PlaySound("END_RELOAD");
    }

    /// <summary>
    /// Calculates bullet loading.
    /// </summary>
    public void EndAnimation()
    {
        Weapon.CalculateReload();
        Weapon.GetStates.SetState("Reloading", false);
    }
}