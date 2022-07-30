using UnityEngine;

namespace Game.Weapons.Attachments
{
    public class Supressor : Attachment
    {
        [Header("Settings")]
        [SerializeField] private MuzzleFlash MuzzleFlash;
        [SerializeField] private Transform muzzleRoot;
        [SerializeField] private AudioClip[] fireSounds;

        protected override void OnEnable()
        {
            base.OnEnable();
            Weapon.SetOverrideFireSound(fireSounds[Random.Range(0, fireSounds.Length)]);

            if (MuzzleFlash != null && muzzleRoot != null)
            {
                MuzzleFlash.SetRoot(muzzleRoot.position);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Weapon.SetOverrideFireSound(null);

            if (MuzzleFlash != null && muzzleRoot != null)
            {
                MuzzleFlash.ResetRoot();
            }
        }
    }
}