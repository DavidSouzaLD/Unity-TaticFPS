using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Code.Interfaces;
using Code.Extensions;

namespace Code.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerCompass : MonoBehaviour, IPlayerControllerComponent
    {
        [Serializable]
        public class Marker
        {
            [Header("Components")]
            public string markerName;
            public Color markerColor;
            public Vector3 position;

            public Transform transform { get; set; }
            public RectTransform rectTransform { get; set; }
            public TextMeshProUGUI markerText { get; set; }
            public Image image { get; set; }

            public float yPosition => rectTransform.anchoredPosition.y;
            public float xPosition => rectTransform.anchoredPosition.x;

            public Marker(string _markerName, RectTransform _rect, Image _image, Color _color, Vector3 _position, TextMeshProUGUI _markerText, Transform _tranform)
            {
                markerName = _markerName;
                rectTransform = _rect;
                image = _image;
                markerColor = _color;
                position = _position;
                markerText = _markerText;
                transform = _tranform;
            }
        }

        [Header("Settings")]
        public RawImage texture;

        [Header("Markers")]
        public GameObject markerPrefab;
        public float markerHeight;
        public List<Marker> markers = new List<Marker>();
        public PlayerController playerController { get; set; }
        private float halfSize = 600;

        private void LateUpdate()
        {
            if (texture.texture != null)
            {
                texture.uvRect = new Rect(playerController.localYRotation / 360f, 0, 1f, 1f);
            }

            foreach (Marker obj in markers)
            {
                var distance = Vector3.Distance(obj.position, playerController.transform.position);
                var angle = CalculateAngle(playerController.transform.position, obj.position);

                obj.markerText.text = "(" + distance.ToString("0") + ")";
                obj.rectTransform.anchoredPosition = new Vector2(AngleToCompassPos(angle), obj.yPosition);
                TransparentEffect(obj.image, obj.xPosition);
                ScaleEffect(obj.rectTransform, playerController.transform.position, obj.position, 20f);
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
            return Vector2.SignedAngle(objectXZ - playerXZ, playerController.forwardXZ);
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

        public void AddMarker(string _name, Vector3 _position)
        {
            Color _color = ColorExtension.GetRandomColor();

            for (int i = 0; i < markers.Count; i++)
            {
                while (_color == markers[i].markerColor)
                {
                    _color = ColorExtension.GetRandomColor();
                }

                if (markers[i].markerName == _name)
                {
                    return;
                }
            }

            GameObject markerObj = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity);
            RectTransform markerRect = markerObj.GetComponent<RectTransform>();
            Image markerImg = markerObj.GetComponent<Image>();
            TextMeshProUGUI markerText = markerObj.GetComponentInChildren<TextMeshProUGUI>();

            markerObj.transform.SetParent(transform);
            markerImg.color = _color;
            markerRect.localPosition = new Vector3(0f, markerHeight, 0f);

            markers.Add(new Marker(_name, markerRect, markerImg, _color, _position, markerText, markerObj.transform));
        }

        public void RemoveMarker(string _name)
        {
            for (int i = 0; i < markers.Count; i++)
            {
                if (markers[i].markerName == _name)
                {
                    Destroy(markers[i].transform.gameObject);
                    markers.RemoveAt(i);
                    return;
                }
            }
        }
    }
}