using UnityEngine;
using Game.WeaponSystem.Components;

namespace Game.WeaponSystem
{

    [CreateAssetMenu(fileName = "ImpactsData", menuName = "PPM22/Weapon/ImpactsData", order = 0)]
    public class ImpactsData : ScriptableObject
    {
        public Impacts.Impact[] impacts;
    }
}