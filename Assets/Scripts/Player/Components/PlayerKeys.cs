using UnityEngine;

namespace Game.Player.Components
{
    public class PlayerKeys : Singleton<PlayerKeys>
    {
        [System.Serializable]
        public class BoolKey
        {
            public string name;
            public KeyCode[] value;

            public bool press
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

            public bool click
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

        [System.Serializable]
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

        [System.Serializable]
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

        public PlayerKeysData data;

        public static bool Press(string name)
        {
            foreach (BoolKey key in Instance.data.boolKeys)
            {
                if (name == key.name)
                {
                    return key.press;
                }
            }

            return false;
        }

        public static bool Click(string name)
        {
            foreach (BoolKey key in Instance.data.boolKeys)
            {
                if (name == key.name)
                {
                    return key.click;
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
}