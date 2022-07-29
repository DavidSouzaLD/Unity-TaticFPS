using UnityEngine;

namespace Game.Weapons.Attachments
{
    public class Sight : MonoBehaviour
    {
        [Header("Settings")]
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
            Weapon.SetAimSensitivityScale(sensitivityScale);
            Weapon.SetAimPosition(aimPosition);
            Weapon.SetMuzzleRoot(newMuzzlePoint);
        }

        private void OnDisable()
        {
            Weapon.MaxAimSensitivityScale();
            Weapon.ResetAimPosition();
            Weapon.ResetMuzzleRoot();
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