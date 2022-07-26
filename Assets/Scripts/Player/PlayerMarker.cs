using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMarker : MonoBehaviour
{
    public bool isCreating;
    public LayerMask hittableLayers;
    public GameObject panelPrefab;
    public TMP_InputField inputField;
    public Button createButton, cancelButton, removeButton;

    private string markName;
    private Vector3 markPosition;
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = FindManager.Find("Camera", this);
        createButton.onClick.AddListener(CreateMarker);
        cancelButton.onClick.AddListener(CancelMarker);
        removeButton.onClick.AddListener(RemoveMarker);
    }

    private void Update()
    {
        // Compass Marker
        if (!LockManager.IsLocked("PLAYER_ALL") && !LockManager.IsLocked("PLAYER_MARKER") && InputManager.Marker && !isCreating)
        {
            RaycastHit hit;

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity, hittableLayers))
            {
                isCreating = true;
                markPosition = hit.point;
            }
        }

        if (isCreating)
        {
            markName = inputField.text;
            panelPrefab.SetActive(true);
            PlayerCamera.LockCursor(false);
        }
        else
        {
            panelPrefab.SetActive(false);
            PlayerCamera.LockCursor(true);
        }

        LockManager.Lock("PLAYER_MARKER", "PLAYER_ALL", isCreating);
        LockManager.Lock("PLAYER_MARKER", "WEAPON_ALL", isCreating);
    }

    private void CreateMarker()
    {
        CompassManager.AddMarker(markName, markPosition);

        markName = "";
        markPosition = Vector3.zero;
        inputField.text = markName;

        isCreating = false;
    }

    private void CancelMarker()
    {
        markName = "";
        markPosition = Vector3.zero;
        inputField.text = markName;

        isCreating = false;
    }

    private void RemoveMarker()
    {
        CompassManager.RemoveMarker(markName);
    }
}
