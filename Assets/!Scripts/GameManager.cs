using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        transform.parent = null;
    }
}