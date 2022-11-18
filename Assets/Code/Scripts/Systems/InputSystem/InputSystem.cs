using System;
using UnityEngine;

namespace Code.Systems.InputSystem
{
    public class InputSystem : MonoBehaviour
    {
        public InputData data;
        private static InputSystem Instance;

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

        public static bool OnPressing(string name)
        {
            foreach (BoolKey key in Instance.data.boolKeys)
            {
                if (name == key.name)
                {
                    return key.onPressing;
                }
            }

            return false;
        }

        public static bool OnClick(string name)
        {
            foreach (BoolKey key in Instance.data.boolKeys)
            {
                if (name == key.name)
                {
                    return key.onClick;
                }
            }

            return false;
        }

        public static float GetFloat(string name)
        {
            foreach (FloatKey key in Instance.data.floatKeys)
            {
                if (name == key.name)
                {
                    return key.value;
                }
            }
            return 0;
        }

        public static Vector2 GetAxis(string name)
        {
            foreach (AxisKey key in Instance.data.axisKeys)
            {
                if (name == key.name)
                {
                    return key.value;
                }
            }
            return Vector2.zero;
        }

        public static Vector2 GetAxisRaw(string name)
        {
            foreach (AxisKey key in Instance.data.axisKeys)
            {
                if (name == key.name)
                {
                    return key.valueRaw;
                }
            }
            return Vector2.zero;
        }
    }

    [Serializable]
    public class BoolKey
    {
        public string name;
        public KeyCode[] value;

        public bool onPressing
        {
            get
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (Input.GetKey(value[i]))
                    {
                        return Input.GetKey(value[i]);
                    }
                }

                return false;
            }
        }

        public bool onClick
        {
            get
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (Input.GetKeyDown(value[i]))
                    {
                        return Input.GetKeyDown(value[i]);
                    }
                }

                return false;
            }
        }
    }

    [Serializable]
    public class FloatKey
    {
        public string name;
        public KeyCode minValue, maxValue;

        public float value
        {
            get
            {
                float value = 0;

                if (Input.GetKey(minValue))
                {
                    value -= 1;
                }

                if (Input.GetKey(maxValue))
                {
                    value += 1;
                }

                return Mathf.Clamp(value, -1, 1);
            }
        }
    }

    [Serializable]
    public class AxisKey
    {
        public string name;
        public string AxisX, AxisY;

        public Vector2 value
        {
            get
            {
                Vector2 axis = new Vector2(
                    Input.GetAxis(AxisX),
                    Input.GetAxis(AxisY)
                );

                return axis;
            }
        }

        public Vector2 valueRaw
        {
            get
            {
                Vector2 axis = new Vector2(
                    Input.GetAxisRaw(AxisX),
                    Input.GetAxisRaw(AxisY)
                );

                return axis;
            }
        }
    }
}