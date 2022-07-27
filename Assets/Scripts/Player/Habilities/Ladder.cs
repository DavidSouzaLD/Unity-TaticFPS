using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private bool inLadder;
    [SerializeField] private Vector3 ladderDirection;
    private bool playerEnter;
    private bool changeControl;

    private void Update()
    {
        if (InputManager.Interact)
        {
            inLadder = !inLadder;
        }

        if (inLadder)
        {
            if (!changeControl)
            {
                PlayerController.GetFunctions.SetAdditionalDirection(ladderDirection);
                PlayerController.GetStates.SetState("Climbing", true);
                PlayerController.GetStates.SetState("Graviting", false);
                changeControl = true;
            }
        }

        if (inLadder == false && changeControl)
        {
            PlayerController.GetFunctions.ResetAdditionalDirection();
            PlayerController.GetStates.SetState("Climbing", true);
            PlayerController.GetStates.SetState("Graviting", true);

            changeControl = false;
            inLadder = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerEnter = true;
            inLadder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inLadder = false;

            PlayerController.GetFunctions.ResetAdditionalDirection();
            PlayerController.GetStates.SetState("Climbing", true);
            PlayerController.GetStates.SetState("Graviting", true);

            playerEnter = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (ladderDirection != Vector3.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, ladderDirection);
        }
    }
}
