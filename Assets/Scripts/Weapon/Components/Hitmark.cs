using System.Collections;
using UnityEngine;
using WeaponSystem.Core;

namespace WeaponSystem.Components
{
    public class Hitmark : WeaponComponent
    {
        [Header("Settings")]
        public GameObject hitmarkObject;
        public AudioClip hitmarkSound;
        public float hitmarkTime;

        public override void Start()
        {
            // Disable hitmark
            hitmarkObject.SetActive(false);
        }

        public override void Update()
        {

        }

        public void ApplyHitMark(Vector3 hitmarkPositon)
        {
            WeaponManager.GetBehaviour.StartCoroutine(ApplyHitmark(hitmarkPositon));
        }

        private IEnumerator ApplyHitmark(Vector3 hitmarkPosition)
        {
            // Active hitmark
            hitmarkObject.transform.position = hitmarkPosition;
            hitmarkObject.SetActive(true);

            yield return new WaitForSeconds(hitmarkTime);

            // Disable hitmark
            hitmarkObject.SetActive(false);
        }
    }
}