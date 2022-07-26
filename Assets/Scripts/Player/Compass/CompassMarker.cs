using UnityEngine;
using UnityEngine.UI;

public class CompassMarker : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image image;
    public Transform objecRefence;
    public Sprite sprite;

    public float yPosition => rectTransform.anchoredPosition.y;
    public float xPosition => rectTransform.anchoredPosition.x;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = sprite;
        rectTransform = GetComponent<RectTransform>();
    }
}