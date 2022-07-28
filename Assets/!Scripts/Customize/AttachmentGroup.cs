using UnityEngine;

public class AttachmentGroup : MonoBehaviour
{
    [System.Serializable]
    public class Attachment
    {
        public string name;
        public GameObject prefab;
    }

    public Attachment[] Attachments;

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
