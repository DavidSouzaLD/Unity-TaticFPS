using UnityEngine;

namespace WeaponSystem.Components
{
    public class Recoil : WeaponComponent
    {
        [Header("Settings")]
        public float resetSpeed = 5f;
        public float camResetSpeed = 8f;

        [Header("Roots")]
        public Transform recoilRoot;
        public Transform camRecoilRoot;
        private Vector3 initialRecoilPos;
        private Quaternion initialRecoilRot;
        private Vector3 camCurrentRotation;
        private Vector3 camTargetRotation;

        public override void Start()
        {
            initialRecoilPos = recoilRoot.localPosition;
            initialRecoilRot = recoilRoot.localRotation;
        }

        public override void Update()
        {
            if (recoilRoot != null)
            {
                if (recoilRoot.localPosition != initialRecoilPos || recoilRoot.localRotation != initialRecoilRot)
                {
                    recoilRoot.localPosition = Vector3.Lerp(recoilRoot.localPosition, initialRecoilPos, resetSpeed * Time.deltaTime);
                    recoilRoot.localRotation = Quaternion.Slerp(recoilRoot.localRotation, initialRecoilRot, resetSpeed * Time.deltaTime);
                }
            }

            if (camRecoilRoot != null)
            {
                camTargetRotation = Vector3.Lerp(camTargetRotation, Vector3.zero, camResetSpeed * Time.deltaTime);
                camCurrentRotation = Vector3.Slerp(camCurrentRotation, camTargetRotation, camResetSpeed * Time.deltaTime);
                camRecoilRoot.localRotation = Quaternion.Euler(camCurrentRotation);
            }
        }

        public void ApplyRecoil(Vector3 recoilForcePos, Vector3 recoilForceRot, Vector3 recoilForceCam)
        {
            Vector3 pos = new Vector3(
            Random.Range(recoilForcePos.x / 2f, recoilForcePos.x),
            Random.Range(recoilForcePos.y / 2f, recoilForcePos.y),
            Random.Range(recoilForcePos.z / 2f, recoilForcePos.z));

            Vector3 rot = new Vector3(
                Random.Range(recoilForceRot.x / 2f, recoilForceRot.x),
                Random.Range(recoilForceRot.y / 2f, recoilForceRot.y),
                Random.Range(recoilForceRot.z / 2f, recoilForceRot.z));

            recoilRoot.localPosition += pos;
            recoilRoot.localRotation *= Quaternion.Euler(rot);

            camTargetRotation += new Vector3(
                -recoilForceCam.x,
                Random.Range(-recoilForceCam.y, recoilForceCam.y),
                Random.Range(-recoilForceCam.z, recoilForceCam.z));
        }
    }
}