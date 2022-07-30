using UnityEngine;

public static class RaycastExtension
{
    public static RaycastHit Raycast(Vector3 _position, Vector3 _direction, float _distance, LayerMask _mask)
    {
        RaycastHit hit;

        if (Physics.Raycast(_position, _direction, out hit, _distance, _mask))
        {
            if (hit.collider != null)
            {
                return hit;
            }
        }

        return hit;
    }
}