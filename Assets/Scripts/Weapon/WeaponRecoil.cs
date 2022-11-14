using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapon
{
    public class WeaponRecoil : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float weaponResetSpeed = 5f;
        [SerializeField] private float camResetSpeed = 8f;

        [Header("Locations")]
        [SerializeField] private Transform recoilLocation;
        [SerializeField] private Transform camRecoilLocation;

        private Vector3 initialRecoilPos;
        private Quaternion initialRecoilRot;
        private Vector3 camCurrentRotation;
        private Vector3 camTargetRotation;
        private void Start()
        {
            initialRecoilPos = recoilLocation.localPosition;
            initialRecoilRot = recoilLocation.localRotation;
        }

        private void Update()
        {
            // Weapon reset recoil
            if (recoilLocation.localPosition != initialRecoilPos || recoilLocation.localRotation != initialRecoilRot)
            {
                recoilLocation.localPosition = Vector3.Lerp(recoilLocation.localPosition, initialRecoilPos, weaponResetSpeed * Time.deltaTime);
                recoilLocation.localRotation = Quaternion.Slerp(recoilLocation.localRotation, initialRecoilRot, weaponResetSpeed * Time.deltaTime);
            }

            // Camera reset recoil
            if (camRecoilLocation)
            {
                camTargetRotation = Vector3.Lerp(camTargetRotation, Vector3.zero, camResetSpeed * Time.deltaTime);
                camCurrentRotation = Vector3.Slerp(camCurrentRotation, camTargetRotation, camResetSpeed * Time.deltaTime);
                camRecoilLocation.localRotation = Quaternion.Euler(camCurrentRotation);
            }
        }

        public void ApplyRecoil(Vector3 _recoilForcePos, Vector3 _recoilForceRot, Vector3 _recoilForceCam)
        {
            Vector3 pos = new Vector3(
            Random.Range(_recoilForcePos.x / 2f, _recoilForcePos.x),
            Random.Range(_recoilForcePos.y / 2f, _recoilForcePos.y),
            Random.Range(_recoilForcePos.z / 2f, _recoilForcePos.z));

            Vector3 rot = new Vector3(
                Random.Range(_recoilForceRot.x / 2f, _recoilForceRot.x),
                Random.Range(_recoilForceRot.y / 2f, _recoilForceRot.y),
                Random.Range(_recoilForceRot.z / 2f, _recoilForceRot.z));

            recoilLocation.localPosition += pos;
            recoilLocation.localRotation *= Quaternion.Euler(rot);

            camTargetRotation += new Vector3(
                -_recoilForceCam.x,
                Random.Range(-_recoilForceCam.y, _recoilForceCam.y),
                Random.Range(-_recoilForceCam.z, _recoilForceCam.z));
        }
    }
}