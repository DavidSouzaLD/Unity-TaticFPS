using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MuzzleFlash : MonoBehaviour
{
    public Sprite[] muzzleSprites;
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
