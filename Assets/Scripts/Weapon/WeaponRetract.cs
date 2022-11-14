using UnityEngine;

namespace Game.Weapon
{
    public class WeaponRetract : MonoBehaviour
    {
        [Header("Settings")]
        public LayerMask rectractMask;
        public float retractRayDistance = 1f;
        public float retractPosition = -5f;
        public float retractAngle = -20f;
        public float retractSpeed = 6f;

        [Header("Locations")]
        public Transform retractRayLocation;
        public Transform retracLocation;

        // Private
        public bool isRetracting { get; private set; }
        private Vector3 initialRetractPos;
        private Quaternion initialRetractRot;

        private void Start()
        {
            initialRetractPos = retracLocation.localPosition;
            initialRetractRot = retracLocation.localRotation;
        }

        private void Update()
        {
            RaycastHit hit;
            Debug.DrawRay(retractRayLocation.position, retractRayLocation.forward * retractRayDistance, Color.red);

            if (Physics.Raycast(retractRayLocation.position, retractRayLocation.forward, out hit, retractRayDistance, rectractMask))
            {
                if (hit.transform)
                {
                    isRetracting = true;
                    Vector3 targetPos = new Vector3(retracLocation.localPosition.x, retracLocation.localPosition.y, ((retractPosition * 0.001f) / hit.distance));
                    Quaternion targetRot = Quaternion.Euler(new Vector3((retractAngle / hit.distance), initialRetractRot.y, initialRetractRot.z));
                    retracLocation.localPosition = Vector3.Lerp(retracLocation.localPosition, targetPos, retractSpeed * Time.deltaTime);
                    retracLocation.localRotation = Quaternion.Slerp(retracLocation.localRotation, targetRot, retractSpeed * Time.deltaTime);
                }
            }
            else
            {
                isRetracting = false;
            }

            retracLocation.localPosition = Vector3.Lerp(retracLocation.localPosition, initialRetractPos, retractSpeed * Time.deltaTime);
            retracLocation.localRotation = Quaternion.Slerp(retracLocation.localRotation, initialRetractRot, retractSpeed * Time.deltaTime);
        }
    }
}