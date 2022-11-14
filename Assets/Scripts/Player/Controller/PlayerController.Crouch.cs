using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public partial class PlayerController
    {
        protected void UpdateCrouch()
        {
            // Crouch
            bool inputConditions = PlayerKeys.Press("Crouch") && !PlayerKeys.Press("Run");

            if (inputConditions)
            {
                controller.LerpHeight(crouchHeight, speedToCrouch * Time.deltaTime);
                camRoot.transform.localPosition = new Vector3(
                    camRoot.transform.localPosition.x,
                    Mathf.Lerp(camRoot.transform.localPosition.y, crouchHeight / 2f, speedToCrouch * Time.deltaTime),
                camRoot.transform.localPosition.z
                );

                isCrouching = true;
            }
            else
            {
                controller.LerpHeight(standHeight, speedToCrouch * Time.deltaTime);
                camRoot.transform.localPosition = new Vector3(
                    camRoot.transform.localPosition.x,
                    Mathf.Lerp(camRoot.transform.localPosition.y, standHeight / 2f, speedToCrouch * Time.deltaTime),
                camRoot.transform.localPosition.z
                );

                isCrouching = false;
            }
        }
    }
}