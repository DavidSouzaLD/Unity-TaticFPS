using UnityEngine;
using Game.Player.Components;

namespace Game.Player
{
    [CreateAssetMenu(fileName = "PlayerKeysData", menuName = "PPM22/Player/PlayerKeysData", order = 0)]
    public class PlayerKeysData : ScriptableObject
    {
        public PlayerKeys.BoolKey[] boolKeys;
        public PlayerKeys.FloatKey[] floatKeys;
        public PlayerKeys.AxisKey[] axisKeys;
    }
}