using UnityEngine;

public class WeaponAnimEvents : MonoBehaviour
{
    protected WeaponSystem WeaponSystem;

    private void Start()
    {
        WeaponSystem = GetComponentInParent<WeaponSystem>();
    }

    public void StartEvent()
    {
        WeaponSystem.ApplySound("START_RELOAD");
        WeaponSystem.isReloading = true;
    }

    public void MiddleEvent()
    {
        WeaponSystem.ApplySound("MIDDLE_RELOAD");
    }

    public void EndEvent()
    {
        WeaponSystem.ApplySound("END_RELOAD");
        WeaponSystem.ApplyReload();
        WeaponSystem.isReloading = false;
    }
}