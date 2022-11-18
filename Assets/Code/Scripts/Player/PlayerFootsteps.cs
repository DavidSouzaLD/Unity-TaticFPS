using System.Linq;
using UnityEngine;
using Code.Systems.AudioSystem;
using Code.Interfaces;

namespace Code.Player
{
    public class PlayerFootsteps : MonoBehaviour, IPlayerControllerComponent
    {
        [System.Serializable]
        public class Footsteps
        {
            public string tagName;
            public AudioClip[] clips;
        }

        public Footsteps[] footsteps;
        public PlayerController playerController { get; set; }
        private float footstepTimer;

        public void SetPlayerController(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public Footsteps GetFootstepsWithTag(string tagName)
        {
            var sound1 =
            from sound in footsteps
            where sound.tagName.ToUpper() == tagName.ToUpper()
            select sound;

            return sound1?.ToArray()[0];
        }

        private void Update()
        {
            bool conditions = PlayerController.isWalking;

            if (conditions)
            {
                float footstepSpeed = 0;

                if (!PlayerController.isCrouching)
                {
                    footstepSpeed = playerController.data.crouchStepSpeed;
                }
                else
                {
                    footstepSpeed = PlayerController.isRunning ? playerController.data.runStepSpeed : playerController.data.walkStepSpeed;
                }

                if (footstepTimer <= 0)
                {
                    AudioClip[] clips = GetFootstepsWithTag(PlayerController.currentGroundTag).clips;

                    if (clips.Length > 0)
                    {
                        AudioSystem.PlaySound(clips[Random.Range(0, clips.Length)], playerController.data.footstepVolume);
                    }

                    footstepTimer = footstepSpeed;
                }
            }

            footstepTimer -= Time.deltaTime;
        }
    }
}