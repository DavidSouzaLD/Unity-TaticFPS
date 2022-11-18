using UnityEngine;

namespace Code.Systems.InputSystem
{
    [CreateAssetMenu(fileName = "InputData", menuName = "Systems/InputData", order = 0)]
    public class InputData : ScriptableObject
    {
        public BoolKey[] boolKeys;
        public FloatKey[] floatKeys;
        public AxisKey[] axisKeys;
    }
}