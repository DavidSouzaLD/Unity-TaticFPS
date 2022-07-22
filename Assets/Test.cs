using UnityEngine;

public class Test : MonoBehaviour
{
    public float speed = 400f;
    public Vector3 bulletVelocity;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 point1 = this.transform.position;
        Vector3 predictedBulletVelocity = transform.forward * speed;
        float stepSize = 0.01f;

        for (float step = 0f; step < 1; step += stepSize)
        {
            predictedBulletVelocity += Physics.gravity * stepSize;
            Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
            Gizmos.DrawLine(point1, point2);

            Ray ray = new Ray(point1, point2 - point1);

            if (Physics.Raycast(ray, (point2 - point1).magnitude))
            {
                Debug.Log("Hit");
            }

            point1 = point2;
        }
    }
}
