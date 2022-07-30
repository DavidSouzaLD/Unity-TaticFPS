using UnityEngine;

namespace Game.Weapons
{
    public class WeaponRetract : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask rectractMask;
        [SerializeField] private float retractRayDistance = 1f;
        [SerializeField] private float retractAngle = -20f;
        [SerializeField] private float retractSpeed = 6f;

        [Header("Roots")]
        [SerializeField] private Transform retractRayRoot;
        [SerializeField] private Transform retracRoot;

        // Private
        private bool isRetracting;
        private Quaternion retractInitialRot;

        public bool IsRetracting()
        {
            return isRetracting;
        }

        private void Start()
        {
            retractInitialRot = retracRoot.localRotation;
        }

        private void Update()
        {
            RaycastHit hit;
            Debug.DrawRay(retractRayRoot.position, retractRayRoot.forward * retractRayDistance, Color.red);

            if (Physics.Raycast(retractRayRoot.position, retractRayRoot.forward, out hit, retractRayDistance, rectractMask))
            {
                if (hit.transform)
                {
                    isRetracting = true;
                    Quaternion targetRot = Quaternion.Euler(new Vector3((retractAngle / hit.distance), retractInitialRot.y, retractInitialRot.z));
                    retracRoot.localRotation = Quaternion.Slerp(retracRoot.localRotation, targetRot, retractSpeed * Time.deltaTime);
                }
            }
            else
            {
                isRetracting = false;
            }

            retracRoot.localRotation = Quaternion.Slerp(retracRoot.localRotation, retractInitialRot, retractSpeed * Time.deltaTime);
        }
    }
}