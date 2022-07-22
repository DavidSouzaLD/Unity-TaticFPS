using UnityEngine;

public class WeaponAttachments : MonoBehaviour
{
    public enum AttachmentType { Muzzle, Grip, Handguard, Stock }

    [System.Serializable]
    public class Attachment
    {
        public GameObject prefab;

        public void Active(bool value)
        {
            if (prefab != null)
            {
                prefab.SetActive(value);
            }
        }
    }

    [Header("Equiped")]
    public Attachment Slot;

    [Header("List")]
    public Attachment[] Attachments;

    int currentID;

    public void ChangeAttachment()
    {
        if (currentID < Attachments.Length)
        {
            currentID++;
        }

        if (currentID > Attachments.Length - 1)
        {
            currentID = 0;
        }

        currentID = Mathf.Clamp(currentID, 0, Attachments.Length - 1);

        if (Slot != null)
        {
            Slot.Active(false);
        }

        Slot = Attachments[currentID];
        Slot.Active(true);
    }
}