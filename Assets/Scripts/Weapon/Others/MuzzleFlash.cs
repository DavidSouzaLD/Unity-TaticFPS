using UnityEngine;

namespace Game.Weapon
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MuzzleFlash : MonoBehaviour
    {
        [Header("Settings")]
        public Vector3 initialPosition;
        public Sprite[] muzzleSprites;

        // Private
        private Vector3 defaultPos;
        private SpriteRenderer SpriteRenderer;
        private Weapon Weapon;

        public void SetLocation(Vector3 position)
        {
            transform.position = position;
        }

        public void ResetLocation()
        {
            transform.localPosition = initialPosition;
        }

        private void Start()
        {
            // Get components
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Weapon = GetComponentInParent<Weapon>();

            defaultPos = transform.position;
        }

        private void OnEnable()
        {
            if (SpriteRenderer)
            {
                SpriteRenderer.sprite = muzzleSprites[Random.Range(0, muzzleSprites.Length)];
            }
            else
            {
                SpriteRenderer = GetComponent<SpriteRenderer>();
            }
        }
    }
}