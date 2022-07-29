using UnityEngine;

namespace Game.Weapons
{
    public class WeaponImpacts : Singleton<WeaponImpacts>
    {
        [System.Serializable]
        public class Impact
        {
            public string name;
            public GameObject prefab;
        }

        [Header("Settings")]
        [SerializeField] private Impact[] impacts;

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