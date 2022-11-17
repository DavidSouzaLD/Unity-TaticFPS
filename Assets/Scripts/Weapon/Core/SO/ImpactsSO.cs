using UnityEngine;
using WeaponSystem.Components;

namespace WeaponSystem.Core
{
    [CreateAssetMenu(fileName = "ImpactsSO", menuName = "WeaponSystem/ImpactsSO", order = 1)]
    public class ImpactsSO : ScriptableObject
    {
        public Impacts.Impact[] impacts;
    }
}