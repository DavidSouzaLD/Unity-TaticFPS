using UnityEngine;

public class AttachmentRuntime : MonoBehaviour
{
    public enum AttachmentType { Muzzle, Sight }
    public Weapon Weapon;
    public AttachmentType attachmentType = AttachmentType.Sight;

    // Sight
    [SerializeField] private Vector3 sightPosition;

    private void Awake()
    {
        Weapon = GameObject.FindObjectOfType<Weapon>();
    }

    private void OnEnable()
    {
        switch (attachmentType)
        {
            case AttachmentType.Sight:
                Weapon.SetAimPosition(sightPosition);
                break;
        }
    }

    private void OnDisable()
    {
        switch (attachmentType)
        {
            case AttachmentType.Sight:
                Weapon.ResetAimPosition();
                break;
        }
    }
}
