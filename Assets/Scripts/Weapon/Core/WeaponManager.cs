using UnityEngine;
using Game.WeaponSystem.Components;

namespace Game.WeaponSystem
{
    public class WeaponManager : Singleton<WeaponManager>
    {
        [Header("Settings")]
        public LayerMask m_hittableMask;
        public Weapon m_currentWeapon;
        public GameObject m_tracerPrefab;

        public static LayerMask hittableMask
        {
            get
            {
                return Instance.m_hittableMask;
            }
        }

        public static Weapon currentWeapon
        {
            get
            {
                return Instance.m_currentWeapon;
            }

            set
            {
                Instance.m_currentWeapon = value;
            }
        }

        public static GameObject tracerPrefab
        {
            get
            {
                return Instance.m_tracerPrefab;
            }
        }

        public static void PlaySound(string soundName, float volume = 1f, AudioClip custom = null)
        {
            int fireSoundLength = Instance.m_currentWeapon.data.fireSounds.Length;
            AudioClip clip = null;

            switch (soundName.ToUpper())
            {
                case "FIRE":

                    if (Instance.m_currentWeapon.overrideFireSound == null)
                    {
                        clip = Instance.m_currentWeapon.data.fireSounds[Random.Range(0, fireSoundLength)];
                    }
                    else
                    {
                        clip = Instance.m_currentWeapon.overrideFireSound;
                    }
                    break;

                case "REMOVING_MAGAZINE": clip = Instance.m_currentWeapon.data.startReloadSound; break;
                case "PUTTING_MAGAZINE": clip = Instance.m_currentWeapon.data.middleReloadSound; break;
                case "COCKING": clip = Instance.m_currentWeapon.data.endReloadSound; break;
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