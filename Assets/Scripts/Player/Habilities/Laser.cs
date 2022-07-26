using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] private bool laserMode;
    [SerializeField] private GameObject pointLight;
    [SerializeField] private float distance;

    [Header("Custom Settings")]
    [SerializeField] private Material laserMaterial;
    [SerializeField] private Color laserColor;

    LineRenderer LineRenderer;
    Camera Camera;

    private void OnValidate()
    {
        if (LineRenderer == null)
        {
            LineRenderer = GetComponent<LineRenderer>();
        }

        if (Camera == null)
        {
            Camera = GetComponentInParent<Camera>();
        }

        if (laserMaterial)
        {
            if (laserMaterial.color != laserColor)
            {
                laserMaterial.color = laserColor;
                laserMaterial.SetColor("_EmissionColor", laserColor);

                if (LineRenderer)
                {
                    LineRenderer.material = laserMaterial;
                }

                if (pointLight.GetComponent<MeshRenderer>())
                {
                    pointLight.GetComponent<MeshRenderer>().material = laserMaterial;
                }
            }
        }
    }

    private void Start()
    {
        LineRenderer.enabled = laserMode;
    }

    private void LateUpdate()
    {
        if (InputManager.Laser)
        {
            laserMode = !laserMode;
            LineRenderer.enabled = laserMode;
        }

        if (laserMode)
        {
            if (Camera && pointLight && LineRenderer)
            {
                RaycastHit hit;
                LineRenderer.SetPosition(1, new Vector3(0f, 0f, distance));

                if (Physics.Raycast(transform.position, transform.forward, out hit, distance))
                {
                    if (hit.transform)
                    {
                        if (!pointLight.activeSelf)
                        {
                            pointLight.SetActive(true);
                        }

                        pointLight.transform.position = hit.point;
                    }
                    else
                    {
                        if (pointLight.activeSelf)
                        {
                            pointLight.SetActive(false);
                        }
                    }
                }
                else
                {
                    if (pointLight.activeSelf)
                    {
                        pointLight.SetActive(false);
                    }
                }
            }
        }
        else
        {
            if (pointLight.activeSelf)
            {
                pointLight.SetActive(false);
            }
        }
    }
}
