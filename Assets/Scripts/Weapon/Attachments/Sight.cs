using UnityEngine;

namespace Game.Weapons.Attachments
{
    public class Sight : Attachment
    {
        [Header("Settings")]
        [SerializeField] private Vector3 aimPosition;
        [SerializeField] private Transform newMuzzlePoint;
        [SerializeField, Range(0f, 1f)] private float sensitivityScale;

        protected override void OnEnable()
        {
            base.OnEnable();
            Weapon.SetAimSensitivityScale(sensitivityScale);
            Weapon.SetAimPosition(aimPosition);
            Weapon.SetFireRoot(newMuzzlePoint);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Weapon.MaxAimSensitivityScale();
            Weapon.ResetAim();
            Weapon.ResetFireRoot();
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
}