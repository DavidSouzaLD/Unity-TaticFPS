using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private Vector3 RotationAxis;

    private void Update()
    {
        transform.Rotate(RotationAxis * Time.deltaTime);
    }
}