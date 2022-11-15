using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapon
{
    public class Recoil : Singleton<Recoil>
    {
        [Header("Settings")]
        public float weaponResetSpeed = 5f;
        public float camResetSpeed = 8f;

        [Header("Transforms")]
        public Transform recoilTransform;
        public Transform camRecoilTransform;

        private Vector3 initialRecoilPos;
        private Quaternion initialRecoilRot;
        private Vector3 camCurrentRotation;
        private Vector3 camTargetRotation;

        private void Start()
        {
            initialRecoilPos = recoilTransform.localPosition;
            initialRecoilRot = recoilTransform.localRotation;
        }

        private void Update()
        {
            // Weapon reset recoil
            if (recoilTransform.localPosition != initialRecoilPos || recoilTransform.localRotation != initialRecoilRot)
            {
                recoilTransform.localPosition = Vector3.Lerp(recoilTransform.localPosition, initialRecoilPos, weaponResetSpeed * Time.deltaTime);
                recoilTransform.localRotation = Quaternion.Slerp(recoilTransform.localRotation, initialRecoilRot, weaponResetSpeed * Time.deltaTime);
            }

            // Camera reset recoil
            if (camRecoilTransform)
            {
                camTargetRotation = Vector3.Lerp(camTargetRotation, Vector3.zero, camResetSpeed * Time.deltaTime);
                camCurrentRotation = Vector3.Slerp(camCurrentRotation, camTargetRotation, camResetSpeed * Time.deltaTime);
                camRecoilTransform.localRotation = Quaternion.Euler(camCurrentRotation);
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

            Instance.recoilTransform.localPosition += pos;
            Instance.recoilTransform.localRotation *= Quaternion.Euler(rot);

            Instance.camTargetRotation += new Vector3(
                -_recoilForceCam.x,
                Random.Range(-_recoilForceCam.y, _recoilForceCam.y),
                Random.Range(-_recoilForceCam.z, _recoilForceCam.z));
        }
    }
}