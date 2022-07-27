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
                PlayerController.SetAdditionalDirection(ladderDirection);
                PlayerController.SetState("Climbing", true);
                PlayerController.UseGravity = false;
                changeControl = true;
            }
        }

        if (inLadder == false && changeControl)
        {
            PlayerController.ResetAdditionalDirection();
            PlayerController.SetState("Climbing", false);
            PlayerController.UseGravity = true;

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

            PlayerController.ResetAdditionalDirection();
            PlayerController.SetState("Climbing", false);
            PlayerController.UseGravity = true;

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
