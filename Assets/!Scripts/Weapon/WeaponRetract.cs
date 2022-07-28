using UnityEngine;

public class WeaponRetract : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask rectractMask;
    [SerializeField] private float retractRayDistance = 1f;
    [SerializeField] private float retractAngle = -20f;
    [SerializeField] private float retractSpeed = 6f;

    private Quaternion retractInitialRot;
    private Transform retractRayRoot;
    private Transform retracRoot;

    private void Start()
    {
        retracRoot = FindManager.Find("Retract");
        retractRayRoot = FindManager.Find("Camera");
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
                Quaternion targetRot = Quaternion.Euler(new Vector3((retractAngle / hit.distance), retractInitialRot.y, retractInitialRot.z));
                retracRoot.localRotation = Quaternion.Slerp(retracRoot.localRotation, targetRot, retractSpeed * Time.deltaTime);
            }
        }

        retracRoot.localRotation = Quaternion.Slerp(retracRoot.localRotation, retractInitialRot, retractSpeed * Time.deltaTime);
    }
}
