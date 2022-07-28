using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class AttachmentButton : MonoBehaviour
{
    private TextMeshProUGUI TextMeshProUGUI;
    private Button Button;
    private AttachmentGroup.Attachment Attachment;
    private AttachmentGroup AttachmentGroup;

    public string text
    {
        set
        {
            if (TextMeshProUGUI)
            {
                TextMeshProUGUI.text = value;
            }
        }
    }

    private void OnEnable()
    {
        TextMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
        Button = GetComponent<Button>();
        Button.onClick.AddListener(Active);
    }

    public void Parent(Transform _parent)
    {
        transform.SetParent(_parent);
    }

    public void SetAttachment(AttachmentGroup.Attachment _attachment, AttachmentGroup _group)
    {
        Attachment = _attachment;
        AttachmentGroup = _group;
    }

    public void Active()
    {
        if (AttachmentGroup)
        {
            for (int i = 0; i < AttachmentGroup.Attachments.Length; i++)
            {
                AttachmentGroup.Attachments[i].prefab.SetActive(false);
            }
        }

        Attachment.prefab.SetActive(!Attachment.prefab.activeSelf);
    }
}
