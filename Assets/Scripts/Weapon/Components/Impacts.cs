using UnityEngine;
using WeaponSystem.Core;

namespace WeaponSystem.Components
{
    public class Impacts : WeaponComponent
    {
        [System.Serializable]
        public class Impact
        {
            public string name;
            public GameObject prefab;
        }

        public ImpactsSO data;

        public Impact GetImpactWithTag(string tag)
        {
            foreach (Impact impact in data.impacts)
            {
                if (impact.name == tag)
                {
                    return impact;
                }
            }
            return null;
        }

        public override void Start() { }
        public override void Update() { }
    }
}