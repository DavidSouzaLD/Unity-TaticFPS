using UnityEngine;

namespace WeaponSystem.Components
{
    public class Retract : WeaponComponent
    {
        [Header("Settings")]
        public LayerMask rectractMask;
        public float retractRayDistance = 1f;
        public float retractPosition = -5f;
        public float retractAngle = -20f;
        public float retractSpeed = 6f;

        [Header("Roots")]
        public Transform retractRoot;
        public Transform retractRayRoot;
        private Vector3 initialRetractPos;
        private Quaternion initialRetractRot;

        public bool isRetracting { get; private set; }

        public override void Start()
        {
            initialRetractPos = retractRoot.localPosition;
            initialRetractRot = retractRoot.localRotation;
        }

        public override void Update()
        {
            if (retractRoot == null || retractRayRoot == null) return;

            RaycastHit hit;
            Debug.DrawRay(retractRayRoot.position, retractRayRoot.forward * retractRayDistance, Color.red);

            if (Physics.Raycast(retractRayRoot.position, retractRayRoot.forward, out hit, retractRayDistance, rectractMask))
            {
                if (hit.transform)
                {
                    isRetracting = true;
                    Vector3 targetPos = new Vector3(retractRoot.localPosition.x, retractRoot.localPosition.y, ((retractPosition * 0.001f) / hit.distance));
                    Quaternion targetRot = Quaternion.Euler(new Vector3((retractAngle / hit.distance), initialRetractRot.y, initialRetractRot.z));
                    retractRoot.localPosition = Vector3.Lerp(retractRoot.localPosition, targetPos, retractSpeed * Time.deltaTime);
                    retractRoot.localRotation = Quaternion.Slerp(retractRoot.localRotation, targetRot, retractSpeed * Time.deltaTime);
                }
            }
            else
            {
                isRetracting = false;
            }

            retractRoot.localPosition = Vector3.Lerp(retractRoot.localPosition, initialRetractPos, retractSpeed * Time.deltaTime);
            retractRoot.localRotation = Quaternion.Slerp(retractRoot.localRotation, initialRetractRot, retractSpeed * Time.deltaTime);
        }
    }
}