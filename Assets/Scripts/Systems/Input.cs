using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Systems
{
    /// <summary>
    /// Manages and returns the values ​​of pressed buttons and keys;
    /// </summary>
    [DisallowMultipleComponent]
    public class Input : MonoBehaviour
    {
        private static InputMap Map;

        private void Awake()
        {
            Map = new InputMap();
        }

        private void OnEnable()
        {
            Map.Enable();
        }

        private void OnDisable()
        {
            Map.Disable();
        }

        public static bool GetBool(string _keyName)
        {
            InputAction action = Map.FindAction(_keyName);

            if (action == null)
            {
                Debug.LogError("(" + _keyName + ") not found in InputSystem!");
                return false;
            }

            return action.ReadValue<float>() != 0;
        }

        public static float GetFloat(string _keyName)
        {
            InputAction action = Map.FindAction(_keyName);

            if (action == null)
            {
                Debug.LogError("(" + _keyName + ") not found in InputSystem!");
                return 0;
            }

            return action.ReadValue<float>();
        }

        public static Vector2 GetVector2(string _keyName)
        {
            InputAction action = Map.FindAction(_keyName);

            if (action == null)
            {
                Debug.LogError("(" + _keyName + ") not found in InputSystem!");
                return Vector2.zero;
            }

            return action.ReadValue<Vector2>();
        }
    }
}