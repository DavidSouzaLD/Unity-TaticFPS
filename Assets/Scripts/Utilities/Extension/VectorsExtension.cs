using UnityEngine;

public static class VectorsExtension
{
    public static Vector3 InverseTransformPoint(Vector3 transformPos, Quaternion transformRotation, Vector3 transformScale, Vector3 pos)
    {
        Matrix4x4 matrix = Matrix4x4.TRS(transformPos, transformRotation, transformScale);
        Matrix4x4 inverse = matrix.inverse;
        return inverse.MultiplyPoint3x4(pos);
    }
}