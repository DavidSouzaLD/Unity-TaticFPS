using UnityEngine;

namespace Game.Weapon
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class WeaponSound : MonoBehaviour
    {
        // Private
        private Weapon weapon;

        public void Init(Weapon sendWeapon)
        {
            weapon = sendWeapon;
        }

        public void Play(string _soundName, float _volume = 1f)
        {
            int fireSoundLength = weapon.fireSounds.Length;
            AudioClip clip = null;

            switch (_soundName.ToUpper())
            {
                case "FIRE":

                    if (weapon.overrideFireSound == null)
                    {
                        clip = weapon.fireSounds[Random.Range(0, fireSoundLength)];
                    }
                    else
                    {
                        clip = weapon.overrideFireSound;
                    }
                    break;

                case "START_RELOAD": clip = weapon.startReloadSound; break;
                case "MIDDLE_RELOAD": clip = weapon.middleReloadSound; break;
                case "END_RELOAD": clip = weapon.endReloadSound; break;
                case "HITMARK": clip = WeaponHitmark.GetHitMarkSound(); break;
            }

            if (clip != null)
            {
                AudioManager.PlaySound(clip, _volume);
            }
        }
    }
}