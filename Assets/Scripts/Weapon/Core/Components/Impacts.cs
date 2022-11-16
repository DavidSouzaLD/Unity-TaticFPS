using UnityEngine;

namespace Game.Weapon.Components
{
    public class Impacts : Singleton<Impacts>
    {
        [System.Serializable]
        public class Impact
        {
            public string name;
            public GameObject prefab;
        }

        public ImpactsData data;

        public static Impact GetImpactWithTag(string tag)
        {
            foreach (Impact impact in Instance.data.impacts)
            {
                if (impact.name == tag)
                {
                    return impact;
                }
            }
            return null;
        }
    }
}