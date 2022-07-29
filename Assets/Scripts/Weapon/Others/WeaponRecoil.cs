using UnityEngine;

namespace Game.Weapon
{
    [DisallowMultipleComponent]
    public class WeaponRecoil : Singleton<WeaponRecoil>
    {
        [Header("Settings")]
        [SerializeField] private float weaponResetSpeed = 5f;
        [SerializeField] private float camResetSpeed = 8f;

        [Header("Roots")]
        [SerializeField] private Transform recoilRoot;
        [SerializeField] private Transform camRecoilRoot;

        // Private
        private Vector3 initialRecoilPos;
        private Quaternion initialRecoilRot;
        private Vector3 camCurrentRotation;
        private Vector3 camTargetRotation;

        private void Start()
        {
            initialRecoilPos = recoilRoot.localPosition;
            initialRecoilRot = recoilRoot.localRotation;
        }

        private void Update()
        {
            // Weapon reset recoil
            if (recoilRoot.localPosition != initialRecoilPos || recoilRoot.localRotation != initialRecoilRot)
            {
                recoilRoot.localPosition = Vector3.Lerp(recoilRoot.localPosition, initialRecoilPos, weaponResetSpeed * Time.deltaTime);
                recoilRoot.localRotation = Quaternion.Slerp(recoilRoot.localRotation, initialRecoilRot, weaponResetSpeed * Time.deltaTime);
            }

            // Camera reset recoil
            if (camRecoilRoot)
            {
                camTargetRotation = Vector3.Lerp(camTargetRotation, Vector3.zero, camResetSpeed * Time.deltaTime);
                camCurrentRotation = Vector3.Slerp(camCurrentRotation, camTargetRotation, camResetSpeed * Time.deltaTime);
                camRecoilRoot.localRotation = Quaternion.Euler(camCurrentRotation);
            }
        }

        public static void ApplyRecoil(Vector3 _recoilForcePos, Vector3 _recoilForceRot, Vector3 _recoilForceCam)
        {
            Vector3 pos = new Vector3(
            Random.Range(_recoilForcePos.x / 2f, _recoilForcePos.x),
            Random.Range(_recoilForcePos.y / 2f, _recoilForcePos.y),
            Random.Range(_recoilForcePos.z / 2f, _recoilForcePos.z));

            Vector3 rot = new Vector3(
                Random.Range(_recoilForceRot.x / 2f, _recoilForceRot.x),
                Random.Range(_recoilForceRot.y / 2f, _recoilForceRot.y),
                Random.Range(_recoilForceRot.z / 2f, _recoilForceRot.z));

            Instance.recoilRoot.localPosition += pos;
            Instance.recoilRoot.localRotation *= Quaternion.Euler(rot);

            Instance.camTargetRotation += new Vector3(
                -_recoilForceCam.x,
                Random.Range(-_recoilForceCam.y, _recoilForceCam.y),
                Random.Range(-_recoilForceCam.z, _recoilForceCam.z));
        }
    }
}