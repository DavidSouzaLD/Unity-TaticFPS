using UnityEngine;

[DisallowMultipleComponent]
public class DebugManager : MonoBehaviour
{
    private DebugManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static void DebugAssignedError(string behaviour)
    {
        Debug.LogError("(" + behaviour + ") not assigned, please solve.");
    }
}
