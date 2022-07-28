using UnityEngine;

public class PlayerFunctions
{
    private PlayerController Controller;
    private PlayerState ControllerState;

    public PlayerFunctions(PlayerController _playerController, PlayerState _playerState)
    {
        Controller = _playerController;
        ControllerState = _playerState;
    }

    public void Start()
    {
        Transform transform = Controller.transform;

        // Setting starts states
        Controller.States.SetState("Graviting", true);

        // Components
        Controller.Rigidbody = transform.GetComponent<Rigidbody>();
        Controller.CapsuleCollider = transform.GetComponent<CapsuleCollider>();

        // Transforms
        Controller.cameraRoot = FindManager.Find("Camera");
        Controller.coverWeaponRoot = FindManager.Find("WeaponCover");
        Controller.coverCamRoot = FindManager.Find("CameraCover");

        // Jump
        Controller.jumpCountdown = 0.3f;

        // Cover
        Controller.initialCoverRot = Controller.coverWeaponRoot.localRotation;
        Controller.initialCoverCamPos = Controller.coverCamRoot.localPosition;
        Controller.initialCoverCamRot = Controller.coverCamRoot.localRotation;

        // Others
        Controller.initialCapsuleHeight = Controller.CapsuleCollider.height;
        PlayerCamera.LockCursor(true);
    }

    public Vector3 CapsuleTop
    {
        get
        {
            return (Controller.transform.position - Controller.CapsuleCollider.center) + (Controller.transform.up * ((Controller.CapsuleCollider.height / 2f) + Controller.groundAreaHeight));
        }
    }

    public Vector3 CapsuleBottom
    {
        get
        {
            return (Controller.transform.position + Controller.CapsuleCollider.center) - (Controller.transform.up * ((Controller.CapsuleCollider.height / 2f) + Controller.groundAreaHeight));
        }
    }

    public Collider[] GetColliders()
    {
        float radius = Controller.CapsuleCollider.radius + Controller.groundAreaRadius;
        Collider[] hits = Physics.OverlapSphere(CapsuleBottom, radius, Controller.walkableMask);
        return hits;
    }

    public Transform GetTransform()
    {
        return Controller.transform;
    }

    public Vector2 GetForwardXZ()
    {
        return new Vector2(Controller.transform.forward.x, Controller.transform.forward.z);
    }

    public Vector3 GetNormal()
    {
        RaycastHit hit;
        if (Physics.Raycast(CapsuleBottom, -Controller.transform.up, out hit, (Controller.CapsuleCollider.height / 2f) * 1.5f, Controller.walkableMask))
        {
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);
            return hit.normal;
        }
        return Vector3.one;
    }

    public Vector3 GetCoveringPos()
    {
        return new Vector3(Controller.coverAmount * Controller.coverCamScale * PlayerInput.Cover, 0f, 0f);
    }

    public Quaternion GetCoveringRot()
    {
        return Quaternion.Euler(new Vector3(0f, 0f, Controller.coverAmount * -PlayerInput.Cover));
    }
    public float GetLocalYRotation()
    {
        return Controller.transform.localEulerAngles.y; ;
    }

    public float GetSlopeAngle()
    {
        return Vector3.Angle(Controller.transform.up, GetNormal());
    }

    public bool GetState(string stateName)
    {
        return ControllerState.GetState(stateName);
    }

    public void SetState(string stateName, bool value)
    {
        ControllerState.SetState(stateName, value);
    }

    public void UseGravity(bool value)
    {
        Controller.Rigidbody.useGravity = value;
    }
    public void SetAdditionalDirection(Vector3 direction)
    {
        Controller.additionalDirection = direction;
    }

    public void ResetAdditionalDirection()
    {
        Controller.additionalDirection = Vector3.zero;
    }
}
