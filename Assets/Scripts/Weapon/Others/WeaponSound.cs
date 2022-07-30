using UnityEngine;

namespace Game.Weapons
{
    [DisallowMultipleComponent]
    public class WeaponSound : MonoBehaviour
    {
        // Private
        private Weapon Weapon;

        public void Init(Weapon _weapon)
        {
            Weapon = _weapon;
        }

        public void Play(string _soundName, float _volume = 1f)
        {
            int fireSoundLength = Weapon.Preset.fireSounds.Length;
            AudioClip clip = null;

            switch (_soundName.ToUpper())
            {
                case "FIRE":

                    if (Weapon.GetOverrideFireSound() == null)
                    {
                        clip = Weapon.Preset.fireSounds[Random.Range(0, fireSoundLength)];
                    }
                    else
                    {
                        clip = Weapon.GetOverrideFireSound();
                    }
                    break;

                case "START_RELOAD": clip = Weapon.Preset.startReloadSound; break;
                case "MIDDLE_RELOAD": clip = Weapon.Preset.middleReloadSound; break;
                case "END_RELOAD": clip = Weapon.Preset.endReloadSound; break;
                case "HITMARK": clip = WeaponHitmark.GetHitMarkSound(); break;
            }

            if (clip != null)
            {
                Systems.Audio.PlaySound(clip, _volume);
            }
        }
    }
}