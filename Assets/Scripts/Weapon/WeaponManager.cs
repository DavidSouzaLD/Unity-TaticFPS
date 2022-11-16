using UnityEngine;
using Game.Weapon.Components;

namespace Game.Weapon
{
    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager Instance;

        [Header("Settings")]
        public LayerMask hittableMask;
        public Weapon currentWeapon;
        public GameObject tracerPrefab;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void PlaySound(string soundName, float volume = 1f, AudioClip custom = null)
        {
            int fireSoundLength = currentWeapon.data.fireSounds.Length;
            AudioClip clip = null;

            switch (soundName.ToUpper())
            {
                case "FIRE":

                    if (currentWeapon.overrideFireSound == null)
                    {
                        clip = currentWeapon.data.fireSounds[Random.Range(0, fireSoundLength)];
                    }
                    else
                    {
                        clip = currentWeapon.overrideFireSound;
                    }
                    break;

                case "REMOVING_MAGAZINE": clip = currentWeapon.data.startReloadSound; break;
                case "PUTTING_MAGAZINE": clip = currentWeapon.data.middleReloadSound; break;
                case "COCKING": clip = currentWeapon.data.endReloadSound; break;
                case "HITMARK": clip = Hitmark.GetSound; break;
                case "CUSTOM": clip = custom; break;
            }

            if (clip != null)
            {
                AudioManager.PlaySound(clip, volume);
            }
        }
    }
}