using UnityEngine;

public class AttachmentRuntime : MonoBehaviour
{
    public enum AttachmentType { Muzzle, Sight }
    public AttachmentType attachmentType = AttachmentType.Sight;

    // Sight
    [SerializeField] private Vector3 aimPosition;
    [SerializeField] private Transform newMuzzlePoint;

    private Weapon Weapon;

    private void Awake()
    {
        Weapon = GameObject.FindObjectOfType<Weapon>();
    }

    private void OnEnable()
    {
        switch (attachmentType)
        {
            case AttachmentType.Sight:
                Weapon.SetMuzzlePoint(newMuzzlePoint);
                Weapon.SetAimPosition(aimPosition);
                break;
        }
    }

    private void OnDisable()
    {
        switch (attachmentType)
        {
            case AttachmentType.Sight:
                Weapon.ResetMuzzlePoint();
                Weapon.ResetAimPosition();
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (newMuzzlePoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(newMuzzlePoint.position, newMuzzlePoint.forward * 2f);
        }
    }
}
