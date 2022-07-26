using UnityEngine;

public class AttachmentRuntime : MonoBehaviour
{
    public enum AttachmentType { Muzzle, Sight }
    public AttachmentType attachmentType = AttachmentType.Sight;

    [Header("Sight")]
    [SerializeField] private Vector3 aimPosition;
    [SerializeField] private Transform newMuzzlePoint;
    [SerializeField, Range(0f, 1f)] private float sensitivityScale;

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
                Weapon.SetAimSensitivityScale(sensitivityScale);
                Weapon.SetAimPosition(aimPosition);
                Weapon.SetMuzzlePoint(newMuzzlePoint);
                break;
        }
    }

    private void OnDisable()
    {
        switch (attachmentType)
        {
            case AttachmentType.Sight:
                Weapon.ResetAimSensitivityScale();
                Weapon.ResetAimPosition();
                Weapon.ResetMuzzlePoint();
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
