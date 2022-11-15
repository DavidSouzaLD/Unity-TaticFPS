using UnityEngine;

namespace Game.Weapon
{
    public class Retract : Singleton<Retract>
    {
        [Header("Settings")]
        public LayerMask rectractMask;
        public float retractRayDistance = 1f;
        public float retractPosition = -5f;
        public float retractAngle = -20f;
        public float retractSpeed = 6f;

        [Header("Transforms")]
        public Transform retractTransform;
        public Transform retractRayTransform;

        private Vector3 initialRetractPos;
        private Quaternion initialRetractRot;
        public static bool isRetracting { get; private set; }

        private void Start()
        {
            initialRetractPos = retractTransform.localPosition;
            initialRetractRot = retractTransform.localRotation;
        }

        private void Update()
        {
            RaycastHit hit;
            Debug.DrawRay(retractRayTransform.position, retractRayTransform.forward * retractRayDistance, Color.red);

            if (Physics.Raycast(retractRayTransform.position, retractRayTransform.forward, out hit, retractRayDistance, rectractMask))
            {
                if (hit.transform)
                {
                    isRetracting = true;
                    Vector3 targetPos = new Vector3(retractTransform.localPosition.x, retractTransform.localPosition.y, ((retractPosition * 0.001f) / hit.distance));
                    Quaternion targetRot = Quaternion.Euler(new Vector3((retractAngle / hit.distance), initialRetractRot.y, initialRetractRot.z));
                    retractTransform.localPosition = Vector3.Lerp(retractTransform.localPosition, targetPos, retractSpeed * Time.deltaTime);
                    retractTransform.localRotation = Quaternion.Slerp(retractTransform.localRotation, targetRot, retractSpeed * Time.deltaTime);
                }
            }
            else
            {
                isRetracting = false;
            }

            retractTransform.localPosition = Vector3.Lerp(retractTransform.localPosition, initialRetractPos, retractSpeed * Time.deltaTime);
            retractTransform.localRotation = Quaternion.Slerp(retractTransform.localRotation, initialRetractRot, retractSpeed * Time.deltaTime);
        }
    }
}