using UnityEngine;

namespace Game.Weapon
{
    public class Impacts : Singleton<Impacts>
    {
        [System.Serializable]
        public class Impact
        {
            public string name;
            public GameObject prefab;
        }

        [Header("Settings")]
        public Impact[] impacts;

        public static Impact GetImpactWithTag(string tag)
        {
            foreach (Impact imp in Instance.impacts)
            {
                if (imp.name == tag)
                {
                    return imp;
                }
            }

            return null;
        }
    }
}