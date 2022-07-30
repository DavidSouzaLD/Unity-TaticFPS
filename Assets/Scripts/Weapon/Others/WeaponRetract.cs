using UnityEngine;

namespace Game.Weapons
{
    public class WeaponRetract : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask rectractMask;
        [SerializeField] private float retractRayDistance = 1f;
        [SerializeField] private float retractPosition = -5f;
        [SerializeField] private float retractAngle = -20f;
        [SerializeField] private float retractSpeed = 6f;

        [Header("Roots")]
        [SerializeField] private Transform retractRayRoot;
        [SerializeField] private Transform retracRoot;

        // Private
        private bool isRetracting;
        private Vector3 initialRetractPos;
        private Quaternion initialRetractRot;

        public bool IsRetracting()
        {
            return isRetracting;
        }

        private void Start()
        {
            initialRetractPos = retracRoot.localPosition;
            initialRetractRot = retracRoot.localRotation;
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
                    Vector3 targetPos = new Vector3(retracRoot.localPosition.x, retracRoot.localPosition.y, ((retractPosition * 0.001f) / hit.distance));
                    Quaternion targetRot = Quaternion.Euler(new Vector3((retractAngle / hit.distance), initialRetractRot.y, initialRetractRot.z));
                    retracRoot.localPosition = Vector3.Lerp(retracRoot.localPosition, targetPos, retractSpeed * Time.deltaTime);
                    retracRoot.localRotation = Quaternion.Slerp(retracRoot.localRotation, targetRot, retractSpeed * Time.deltaTime);
                }
            }
            else
            {
                isRetracting = false;
            }

            retracRoot.localPosition = Vector3.Lerp(retracRoot.localPosition, initialRetractPos, retractSpeed * Time.deltaTime);
            retracRoot.localRotation = Quaternion.Slerp(retracRoot.localRotation, initialRetractRot, retractSpeed * Time.deltaTime);
        }
    }
}