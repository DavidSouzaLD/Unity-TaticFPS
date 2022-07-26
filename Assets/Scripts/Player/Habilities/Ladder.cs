using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private bool inLadder;
    [SerializeField] private Vector3 ladderDirection;
    private Player Player;
    private bool changeControl;

    private void Update()
    {
        if (InputManager.Interact)
        {
            inLadder = !inLadder;
        }

        if (Player != null)
        {
            if (inLadder)
            {
                if (!changeControl)
                {
                    Player.SetAdditionalDirection(ladderDirection);
                    Player.isLadder = true;
                    Player.UseGravity = false;
                    changeControl = true;
                }
            }
        }

        if (inLadder == false && changeControl)
        {
            if (Player)
            {
                Player.ResetAdditionalDirection();
                Player.isLadder = false;
                Player.UseGravity = true;
            }

            changeControl = false;
            inLadder = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player = other.GetComponent<Player>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inLadder = false;
            Player.ResetAdditionalDirection();
            Player.isLadder = false;
            Player.UseGravity = true;
            Player = null;
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
