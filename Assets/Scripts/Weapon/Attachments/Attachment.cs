using UnityEngine;

namespace Game.Weapons.Attachments
{
    public class Attachment : MonoBehaviour
    {
        public enum AttachmentType { Muzzle, Handguard, Sight }

        [Header("Attachment")]
        public string attachmentName;
        public AttachmentType attachmentType;

        // Private
        protected Weapon Weapon;

        private void Awake()
        {
            Weapon = GetComponentInParent<Weapon>();
        }

        protected virtual void OnEnable()
        {
            if (Weapon == null)
            {
                Weapon = GetComponentInParent<Weapon>();
            }
        }

        protected virtual void OnDisable()
        {

        }
    }
}