using System.Linq;
using UnityEngine;

namespace Game.Character
{
    [CreateAssetMenu(fileName = "CharacterControllerPreset", menuName = "Game/Create CharacterControllerPreset")]
    public class CharacterControllerPreset : ScriptableObject
    {
        [System.Serializable]
        public class Footsteps
        {
            public string tagName;
            public AudioClip[] clips;
        }

        [Header("Movement")]
        public LayerMask walkableMask;
        public float walkingSpeed = 2f;
        public float runningSpeed = 5f;
        public float crouchingSpeed = 1f;
        [Space]
        public float jumpingForce = 50f;
        public float gravityScale = 2f;

        [Header("Crouch")]
        public float standHeight = 2f;
        public float crouchHeight = 1.5f;
        public float speedToCrouch = 5f;

        [Header("Cover")]
        public float coverAmount = 20f;
        public float coverCamScale = 0.02f;
        public float coverSpeed = 6f;

        [Header("Ground")]
        public float groundRadius;
        public float groundAreaRadius = 0.3f;

        [Header("Footsteps Sounds")]
        public float baseStepSpeed = 0.5f;
        public float runStepSpeed = 0.2f;
        public float crouchStepSpeed = 1.5f;
        [Space]
        public float footstepVolume = 0.5f;

        public Footsteps[] footsteps;
        public Footsteps GetFootstepsWithTag(string _tagName)
        {
            var sound =
            from _sound in footsteps
            where _sound.tagName.ToUpper() == _tagName.ToUpper()
            select _sound;

            return sound.ToArray().Count() > 0 ? sound.ToArray()[0] : null;
        }
    }
}