using System.Collections;
using UnityEngine;

namespace Game.Weapon.Components
{
    public class Hitmark : Singleton<Hitmark>
    {
        [Header("Settings")]
        public GameObject hitmarkObject;
        public AudioClip hitmarkSound;
        public float hitmarkTime;

        public static AudioClip GetSound
        {
            get
            {
                return Instance.hitmarkSound;
            }
        }

        private void Start()
        {
            // Disable hitmark
            hitmarkObject.SetActive(false);
        }

        public static void ApplyHitMark(Vector3 hitmarkPositon)
        => Instance.StartCoroutine(ApplyHitmark(hitmarkPositon));

        private static IEnumerator ApplyHitmark(Vector3 hitmarkPosition)
        {
            // Active hitmark
            Instance.hitmarkObject.transform.position = hitmarkPosition;
            Instance.hitmarkObject.SetActive(true);

            yield return new WaitForSeconds(Instance.hitmarkTime);

            // Disable hitmark
            Instance.hitmarkObject.SetActive(false);
        }
    }
}