using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "PlayerKeysData", menuName = "PPM22/PlayerKeysData", order = 0)]
    public class PlayerKeysData : ScriptableObject
    {
        public PlayerKeys.BoolKey[] boolKeys;
        public PlayerKeys.FloatKey[] floatKeys;
        public PlayerKeys.AxisKey[] axisKeys;
    }
}