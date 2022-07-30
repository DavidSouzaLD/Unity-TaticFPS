using UnityEngine;

namespace Game.Weapons
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MuzzleFlash : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector3 initialPosition;
        [SerializeField] private Sprite[] muzzleSprites;

        // Private
        private Vector3 defaultPos;
        private SpriteRenderer SpriteRenderer;
        private Weapon Weapon;

        public void SetRoot(Vector3 _position)
        {
            transform.position = _position;
        }

        public void ResetRoot()
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