using UnityEngine;

public class PlayerFunctions
{
    private PlayerController Player;
    private PlayerState PlayerState;

    public PlayerFunctions(PlayerController _playerController, PlayerState _playerState)
    {
        Player = _playerController;
        PlayerState = _playerState;
    }

    public void Start()
    {
        // Setting starts states
        Player.States.SetState("Graviting", true);

        // Components
        Transform transform = Player.transform;
        Player.Rigidbody = transform.GetComponent<Rigidbody>();
        Player.CapsuleCollider = transform.GetComponent<CapsuleCollider>();

        // Transforms
        Player.cameraRoot = FindManager.Find("Camera");
        Player.coverWeaponRoot = FindManager.Find("WeaponCover");
        Player.coverCamRoot = FindManager.Find("CameraCover");

        // Jump
        Player.jumpCountdown = 0.3f;

        // Cover
        Player.initialCoverRot = Player.coverWeaponRoot.localRotation;
        Player.initialCoverCamPos = Player.coverCamRoot.localPosition;
        Player.initialCoverCamRot = Player.coverCamRoot.localRotation;

        // Others
        Player.initialCapsuleHeight = Player.CapsuleCollider.height;
        PlayerCamera.LockCursor(true);
    }

    public Vector3 CapsuleTop
    {
        get
        {
            return (Player.transform.position - Player.CapsuleCollider.center) + (Player.transform.up * ((Player.CapsuleCollider.height / 2f) + Player.groundAreaHeight));
        }
    }

    public Vector3 CapsuleBottom
    {
        get
        {
            return (Player.transform.position + Player.CapsuleCollider.center) - (Player.transform.up * ((Player.CapsuleCollider.height / 2f) + Player.groundAreaHeight));
        }
    }

    public Collider[] GetColliders()
    {
        float radius = Player.CapsuleCollider.radius + Player.groundAreaRadius;
        Collider[] hits = Physics.OverlapSphere(CapsuleBottom, radius, Player.walkableMask);
        return hits;
    }

    public Vector3 GetNormal()
    {
        RaycastHit hit;
        if (Physics.Raycast(CapsuleBottom, -Player.transform.up, out hit, (Player.CapsuleCollider.height / 2f) * 1.5f, Player.walkableMask))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            return hit.normal;
        }
        return Vector3.one;
    }

    public Transform GetTransform()
    {
        return Player.transform;
    }

    public Vector2 GetForwardXZ()
    {
        return new Vector2(Player.transform.forward.x, Player.transform.forward.z);
    }

    public float GetLocalYRotation()
    {
        return Player.transform.localEulerAngles.y; ;
    }

    public float GetSlopeAngle()
    {
        return Vector3.Angle(Player.transform.up, GetNormal());
    }

    public bool GetState(string stateName)
    {
        return PlayerState.GetState(stateName);
    }

    public void SetState(string stateName, bool value)
    {
        PlayerState.SetState(stateName, value);
    }

    public void UseGravity(bool value)
    {
        Player.Rigidbody.useGravity = value;
    }
    public void SetAdditionalDirection(Vector3 direction)
    {
        Player.additionalDirection = direction;
    }

    public void ResetAdditionalDirection()
    {
        Player.additionalDirection = Vector3.zero;
    }
}
