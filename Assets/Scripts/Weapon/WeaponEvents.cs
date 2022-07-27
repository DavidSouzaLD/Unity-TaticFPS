using UnityEngine;

public class WeaponEvents : MonoBehaviour
{
    protected Weapon Weapon;

    private void OnDisable()
    {
        Weapon.Functions.GetAnimator().Play("None", -1, 0f);
    }

    private void LateUpdate()
    {
        // Active safe animation
        Weapon.Functions.GetAnimator().SetBool("Safety", Weapon.weaponMode == Weapon.WeaponMode.Safety);
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
        Weapon.States.SetState("Reloading", true);
    }

    /// <summary>
    /// Activates the sound of removing the charger.
    /// </summary>
    public void RemoveMagazine()
    {
        Weapon.Functions.PlaySound("START_RELOAD");
    }

    /// <summary>
    /// Activates the sound of replacing the charger.
    /// </summary>
    public void PutMagazine()
    {
        Weapon.Functions.PlaySound("MIDDLE_RELOAD");
    }

    /// <summary>
    /// Activates the sound of cocking the weapon.
    /// </summary>
    public void Cocking()
    {
        Weapon.Animator.SetBool("NoBullet", false);
        Weapon.Functions.PlaySound("END_RELOAD");
    }

    /// <summary>
    /// Calculates bullet loading.
    /// </summary>
    public void EndAnimation()
    {
        Weapon.Functions.CalculateReload();
        Weapon.States.SetState("Reloading", false);
    }
}