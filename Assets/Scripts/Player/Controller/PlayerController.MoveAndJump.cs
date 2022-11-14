using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public partial class PlayerController
    {
        private void UpdateMoveAndJump()
        {
            isGrounded = controller.isGrounded;

            // Moving
            if (isGrounded && currentVelocity.y < 0)
            {
                currentVelocity.y = 0f;
            }

            isWalking = InputAxis != Vector2.zero;
            isRunning = isWalking && InputAxis.y > 0 && PlayerKeys.Press("Run");

            controller.Move(Direction.normalized * CurrentSpeed * Time.deltaTime);

            // Jump
            if (jumpRequested)
            {
                currentVelocity.y += Mathf.Sqrt(jumpingForce * -3f * (Physics.gravity.y * gravityScale));

                isJumping = true;
                jumpTimer = 0.3f;
                jumpRequested = false;
            }

            // Gravity
            currentVelocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;

            controller.Move(currentVelocity * Time.deltaTime);
        }

        protected void UpdateJump()
        {
            // Jump
            bool conditionsToJump = isGrounded && PlayerKeys.Click("Jump") && jumpTimer <= 0;

            if (conditionsToJump)
            {
                jumpRequested = true;
            }

            if (jumpTimer > 0)
            {
                jumpTimer -= Time.deltaTime;
            }

            if (jumpTimer <= 0 || isGrounded)
            {
                isJumping = false;
            }
        }
    }
}