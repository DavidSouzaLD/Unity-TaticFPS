using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomizeManager : MonoBehaviour
{
    public Transform CameraRoot;
    public Transform WeaponsRoot;
    public float zoomSpeed;
    public float minZoom, maxZoom;
    private static CustomizeManager Instance;

    [Header("[Attachments Settings]")]
    [SerializeField]
    private Transform contentTransform;
    [SerializeField]
    private AttachmentButton buttonPrefab;

    [SerializeField]
    private AttachmentGroup SelectedGroup;

    [SerializeField]
    private List<AttachmentButton> buttons = new List<AttachmentButton>();

    [Header("[Rotation Settings]")]

    [SerializeField]
    private Transform rotationTransform;

    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float rotationSmooth;

    private Camera Camera;
    private Quaternion targetRot;
    private Quaternion startRotationRot;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Camera = Camera.main;
        targetRot = rotationTransform.localRotation;
        startRotationRot = rotationTransform.localRotation;
    }

    private void Update()
    {
        CameraRoot.LookAt(new Vector3(WeaponsRoot.position.x, 0));
        CameraRoot.position += new Vector3(0f, 0f, Input.Scroll * zoomSpeed * Time.deltaTime);

        CameraRoot.position = new Vector3(
            CameraRoot.position.x,
            CameraRoot.position.y,
            Mathf.Clamp(CameraRoot.position.z, minZoom, maxZoom)
        );

        if (Input.CustomizeRotation)
        {
            float rotX = Input.CameraAxis.x * rotationSpeed * Mathf.Deg2Rad;
            float rotY = Input.CameraAxis.y * rotationSpeed * Mathf.Deg2Rad;

            rotationTransform.Rotate(Vector3.up, -rotX);
            //rotationTransform.Rotate(Vector3.right, -rotX);
        }
    }

    public static void SetCustomizeAttchment(AttachmentGroup attachmentGroup)
    {
        for (int i = 0; i < Instance.buttons.Count; i++)
        {
            Destroy(Instance.buttons[i].gameObject);
        }

        Instance.buttons.Clear();
        Instance.SelectedGroup = attachmentGroup;

        for (int i = 0; i < attachmentGroup.Attachments.Length; i++)
        {
            AttachmentButton attbutton = Instantiate(Instance.buttonPrefab.gameObject, Vector3.zero, Quaternion.identity).GetComponent<AttachmentButton>();
            attbutton.SetAttachment(attachmentGroup.Attachments[i], attachmentGroup);
            attbutton.Parent(Instance.contentTransform);
            attbutton.text = attachmentGroup.Attachments[i].name;

            Instance.buttons.Add(attbutton);
        }
    }
}
