using System.Linq;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerFootsteps : MonoBehaviour
    {
        [System.Serializable]
        public class Footsteps
        {
            public string tagName;
            public AudioClip[] clips;
        }

        [Header("Footsteps Sounds")]
        public float walkStepSpeed = 0.5f;
        public float runStepSpeed = 0.2f;
        public float crouchStepSpeed = 1.5f;
        [Space]
        public float footstepVolume = 0.5f;
        public Footsteps[] footsteps;

        private float footstepTimer;


        public Footsteps GetFootstepsWithTag(string _tagName)
        {
            var sound =
            from _sound in footsteps
            where _sound.tagName.ToUpper() == _tagName.ToUpper()
            select _sound;

            return sound.ToArray().Count() > 0 ? sound.ToArray()[0] : null;
        }

        private void Update()
        {
            bool conditions = PlayerController.isWalking;
            footstepTimer -= Time.deltaTime;

            if (conditions)
            {
                float footstepSpeed = 0;

                if (!PlayerController.isCrouching)
                {
                    footstepSpeed = crouchStepSpeed;
                }
                else
                {
                    footstepSpeed = PlayerController.isRunning ? runStepSpeed : walkStepSpeed;
                }

                if (footstepTimer <= 0)
                {
                    AudioClip[] clips = GetFootstepsWithTag(PlayerController.currentGroundTag).clips;

                    if (clips.Length > 0)
                    {
                        AudioManager.PlaySound(clips[Random.Range(0, clips.Length)], footstepVolume);
                    }

                    footstepTimer = footstepSpeed;
                }
            }
        }
    }
}