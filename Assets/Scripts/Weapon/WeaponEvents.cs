using UnityEngine;

public class WeaponEvents : MonoBehaviour
{
    protected Weapon Weapon;

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
        Weapon.isReloading = true;
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
        Weapon.PlaySound("END_RELOAD");
    }

    /// <summary>
    /// Calculates bullet loading.
    /// </summary>
    public void EndAnimation()
    {
        Weapon.CalculateReload();
        Weapon.isReloading = false;
    }
}