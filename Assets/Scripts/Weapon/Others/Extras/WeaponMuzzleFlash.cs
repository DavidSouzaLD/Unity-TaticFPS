using UnityEngine;

namespace Game.Weapons
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponMuzzleFlash : MonoBehaviour
    {
        [SerializeField] private Sprite[] muzzleSprites;
        private SpriteRenderer SpriteRenderer;

        private void Start()
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
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