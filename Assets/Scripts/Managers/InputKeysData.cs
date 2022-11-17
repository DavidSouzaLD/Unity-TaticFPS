using UnityEngine;

[CreateAssetMenu(fileName = "InputKeys", menuName = "Managers/InputKeys", order = 0)]
public class InputKeysData : ScriptableObject
{
    public InputManager.BoolKey[] boolKeys;
    public InputManager.FloatKey[] floatKeys;
    public InputManager.AxisKey[] axisKeys;
}