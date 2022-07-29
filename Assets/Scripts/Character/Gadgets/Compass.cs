using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Character;

namespace Game.Character.Gadgets
{
    public class CompassManager : StaticInstance<CompassManager>
    {
        public RawImage texture;
        public GameObject markerPrefab;
        public float markerHeight;
        public List<Marker> markers = new List<Marker>();
        private float halfSize = 600;

        public static List<Marker> GetMarkers
        {
            get
            {
                return Instance.markers;
            }
        }

        private void Update()
        {
            if (texture.texture != null)
            {
                texture.uvRect = new Rect(FPSCharacterController.GetLocalYRotation() / 360f, 0, 1f, 1f);
            }

            foreach (Marker obj in markers)
            {
                var distance = Vector3.Distance(obj.position, FPSCharacterController.GetTransform().position);
                var angle = CalculateAngle(FPSCharacterController.GetTransform().position, obj.position);

                obj.markerText.text = "(" + distance.ToString("0") + ")";
                obj.rectTransform.anchoredPosition = new Vector2(AngleToCompassPos(angle), obj.yPosition);
                TransparentEffect(obj.image, obj.xPosition);
                ScaleEffect(obj.rectTransform, FPSCharacterController.GetTransform().position, obj.position, 20f);
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
            return Vector2.SignedAngle(objectXZ - playerXZ, FPSCharacterController.GetForwardXZ());
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

        public static void AddMarker(string _name, Vector3 _position)
        {
            Color _color = ColorExtension.GetRandomColor();

            for (int i = 0; i < Instance.markers.Count; i++)
            {
                while (_color == Instance.markers[i].markerColor)
                {
                    _color = ColorExtension.GetRandomColor();
                }

                if (Instance.markers[i].markerName == _name)
                {
                    return;
                }
            }

            GameObject markerObj = Instantiate(Instance.markerPrefab, Vector3.zero, Quaternion.identity);
            RectTransform markerRect = markerObj.GetComponent<RectTransform>();
            Image markerImg = markerObj.GetComponent<Image>();
            TextMeshProUGUI markerText = markerObj.GetComponentInChildren<TextMeshProUGUI>();

            markerObj.transform.SetParent(Instance.transform);
            markerImg.color = _color;
            markerRect.localPosition = new Vector3(0f, Instance.markerHeight, 0f);

            Instance.markers.Add(new Marker(_name, markerRect, markerImg, _color, _position, markerText, markerObj.transform));
        }

        public static void RemoveMarker(string _name)
        {
            for (int i = 0; i < Instance.markers.Count; i++)
            {
                if (Instance.markers[i].markerName == _name)
                {
                    Destroy(Instance.markers[i].transform.gameObject);
                    Instance.markers.RemoveAt(i);
                    return;
                }
            }
        }
    }

    [System.Serializable]
    public class Marker
    {
        [Header("Components")]
        public string markerName;
        public Color markerColor;
        public Vector3 position;

        [HideInInspector] public Transform transform;
        [HideInInspector] public RectTransform rectTransform;
        [HideInInspector] public TextMeshProUGUI markerText;
        [HideInInspector] public Image image;

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
}