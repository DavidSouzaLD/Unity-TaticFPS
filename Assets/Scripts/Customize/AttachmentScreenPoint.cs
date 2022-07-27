using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Button))]
public class AttachmentScreenPoint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string attachmentName;
    private Transform attachmentTransform;
    private AttachmentGroup attachmentGroup;
    private TextMeshProUGUI attachmentText;
    private RectTransform RectTransform;
    private Image Image;
    private Button Button;
    private Camera Camera;

    private bool Enabled
    {
        set
        {
            if (!value)
            {
                Image.enabled = false;
                attachmentText.gameObject.SetActive(false);
            }
            else
            {
                Image.enabled = true;
                attachmentText.gameObject.SetActive(true);
            }
        }
    }

    private void Start()
    {
        // Attachmnet settings
        attachmentName = gameObject.name + "Point";
        attachmentTransform = FindManager.Find(attachmentName);
        attachmentGroup = FindManager.Find(attachmentName).GetComponent<AttachmentGroup>();
        attachmentText = GetComponentInChildren<TextMeshProUGUI>();

        // Get components
        Camera = FindManager.Find("Camera").GetComponent<Camera>();
        RectTransform = GetComponent<RectTransform>();
        Image = GetComponent<Image>();
        Button = GetComponent<Button>();

        // Set text false
        attachmentText.text = gameObject.name;
        attachmentText.gameObject.SetActive(false);

        // Set buttons infos
        Button.onClick.AddListener(SetAttachment);
    }

    private void LateUpdate()
    {
        Enabled = attachmentGroup.Attachments.Length > 0;

        if (attachmentTransform != null)
        {
            if (Camera && RectTransform)
            {
                RectTransform.position = Camera.WorldToScreenPoint(attachmentTransform.position);
            }
        }
    }

    private void SetAttachment()
    {
        CustomizeManager.SetCustomizeAttchment(attachmentGroup);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (attachmentText != null)
        {
            attachmentText.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (attachmentText != null)
        {
            attachmentText.gameObject.SetActive(false);
        }
    }
}
