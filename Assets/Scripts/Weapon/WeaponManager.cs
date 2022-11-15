using UnityEngine;

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
            int fireSoundLength = currentWeapon.fireSounds.Length;
            AudioClip clip = null;

            switch (soundName.ToUpper())
            {
                case "FIRE":

                    if (currentWeapon.overrideFireSound == null)
                    {
                        clip = currentWeapon.fireSounds[Random.Range(0, fireSoundLength)];
                    }
                    else
                    {
                        clip = currentWeapon.overrideFireSound;
                    }
                    break;

                case "REMOVING_MAGAZINE": clip = currentWeapon.startReloadSound; break;
                case "PUTTING_MAGAZINE": clip = currentWeapon.middleReloadSound; break;
                case "COCKING": clip = currentWeapon.endReloadSound; break;
                case "HITMARK": clip = Hitmark.GetHitMarkSound(); break;
                case "CUSTOM": clip = custom; break;
            }

            if (clip != null)
            {
                AudioManager.PlaySound(clip, volume);
            }
        }
    }
}