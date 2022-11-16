using UnityEngine;
using Game.Weapon.Components;

namespace Game.Weapon
{

    [CreateAssetMenu(fileName = "ImpactsData", menuName = "PPM22/Weapon/ImpactsData", order = 0)]
    public class ImpactsData : ScriptableObject
    {
        public Impacts.Impact[] impacts;
    }
}