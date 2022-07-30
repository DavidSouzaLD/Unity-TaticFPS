using UnityEngine;

namespace Game.Weapons.Attachments
{
    public class WeaponAttachments : MonoBehaviour
    {
        [SerializeField] private Attachment[] attachments;

        private Attachment GetAttachment(string _name, Attachment.AttachmentType _type)
        {
            for (int i = 0; i < attachments.Length; i++)
            {
                if (attachments[i].attachmentType == _type)
                {
                    if (attachments[i].name == _name)
                    {
                        return attachments[i];
                    }
                }
            }
            return null;
        }
    }
}