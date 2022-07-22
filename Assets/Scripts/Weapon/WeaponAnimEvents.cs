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
        WeaponSystem.SoundEvent("START_RELOAD");
        WeaponSystem.isReloading = true;
    }

    public void MiddleEvent()
    {
        WeaponSystem.SoundEvent("MIDDLE_RELOAD");
    }

    public void EndEvent()
    {
        WeaponSystem.SoundEvent("END_RELOAD");
        WeaponSystem.ReloadEvent();
        WeaponSystem.isReloading = false;
    }
}