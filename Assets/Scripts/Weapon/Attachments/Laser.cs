using UnityEngine;

namespace Game.Weapons.Attachments
{
    [RequireComponent(typeof(LineRenderer))]
    public class Laser : Attachment
    {
        [Header("Settings")]
        [SerializeField] private bool laserMode;
        [SerializeField] private GameObject pointLight;
        [SerializeField] private float distance;

        protected bool isEnabled;
        protected bool changedMode;
        private LineRenderer LineRenderer;
        private Camera Camera;

        private void Start()
        {
            // Get components
            LineRenderer = GetComponent<LineRenderer>();
            Camera = GetComponentInParent<Camera>();

            LineRenderer.enabled = isEnabled;
        }

        private void Update()
        {
            if (Systems.Input.GetBool("WeaponHandguard"))
            {
                isEnabled = !isEnabled;
                LineRenderer.enabled = isEnabled;
            }

            if (isEnabled)
            {
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
            else
            {
                if (pointLight.activeSelf)
                {
                    pointLight.SetActive(false);
                }
            }
        }
    }
}