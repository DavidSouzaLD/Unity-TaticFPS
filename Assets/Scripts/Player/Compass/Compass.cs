using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public RawImage texture;
    public RectTransform north;
    public RectTransform east;
    public RectTransform west;
    public RectTransform south;
    public List<CompassMarker> markers;

    private float halfSize = 600;
    private Player Player;

    private void Start()
    {
        Player = FindManager.Find("Player", this).GetComponent<Player>();
    }

    private void Update()
    {
        if (texture.texture != null)
        {
            texture.uvRect = new Rect(Player.LocalYRotation / 360f, 0, 1f, 1f);
        }
        else
        {
            var northAngle = -Vector2.SignedAngle(Player.ForwardXZ, Vector2.up);
            var eastAngle = -Vector2.SignedAngle(Player.ForwardXZ, Vector2.right);
            var westAngle = -Vector2.SignedAngle(Player.ForwardXZ, Vector2.left);
            var southAngle = -Vector2.SignedAngle(Player.ForwardXZ, Vector2.down);

            north.anchoredPosition = new Vector2(AngleToCompassPos(northAngle), north.anchoredPosition.y);
            east.anchoredPosition = new Vector2(AngleToCompassPos(eastAngle), north.anchoredPosition.y);
            west.anchoredPosition = new Vector2(AngleToCompassPos(westAngle), north.anchoredPosition.y);
            south.anchoredPosition = new Vector2(AngleToCompassPos(southAngle), north.anchoredPosition.y);

            TransparentEffect(north.GetComponent<Image>(), north.anchoredPosition.x);
            TransparentEffect(east.GetComponent<Image>(), east.anchoredPosition.x);
            TransparentEffect(west.GetComponent<Image>(), west.anchoredPosition.x);
            TransparentEffect(south.GetComponent<Image>(), south.anchoredPosition.x);
        }


        foreach (CompassMarker obj in markers)
        {
            var angle = CalculateAngle(Player.transform.position, obj.objecRefence.transform.position);
            obj.rectTransform.anchoredPosition = new Vector2(AngleToCompassPos(angle), obj.yPosition);

            TransparentEffect(obj.image, obj.xPosition);

            ScaleEffect(obj.rectTransform, Player.transform.position, obj.objecRefence.transform.position, 20f);
        }
    }

    private float AngleToCompassPos(float angle)
    {
        return (halfSize / 180f) * angle;
    }

    private float CalculateAngle(Vector3 playerPosition, Vector3 objectPosition)
    {
        Vector2 playerXZ = new Vector2(playerPosition.x, playerPosition.z);
        Vector2 objectXZ = new Vector2(objectPosition.x, objectPosition.z);
        return Vector2.SignedAngle(objectXZ - playerXZ, Player.ForwardXZ);
    }

    private void TransparentEffect(Image image, float x)
    {
        image.color = new Color(image.color.r,
                                image.color.g,
                                image.color.b, 1.1f - (Mathf.Abs(x) / halfSize));
    }

    private void ScaleEffect(RectTransform rectT, Vector3 player, Vector3 objectRef, float maxDistChange)
    {
        float dist = Vector3.Distance(player, objectRef) / maxDistChange;
        dist = Mathf.Clamp(dist, 0f, 1f);
        rectT.localScale = Vector2.one * Mathf.Lerp(1.2f, 0.5f, dist);
    }

}