using UnityEngine;

namespace WeaponSystem.Core.Others
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MuzzleFlash : MonoBehaviour
    {
        [Header("Settings")]
        public Sprite[] muzzleSprites;

        private Vector3 initialPosition;
        public Vector3 overridePosition { get; set; }
        private SpriteRenderer spriteRenderer { get; set; }

        public void ClearOverridePosition()
        {
            overridePosition = Vector3.zero;
        }

        private void Start()
        {
            // Setting initial positon
            initialPosition = transform.position;
        }

        private void OnEnable()
        {
            // Getting component
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            // Apply muzzle sprite
            spriteRenderer.sprite = muzzleSprites[Random.Range(0, muzzleSprites.Length)];
        }
    }
}