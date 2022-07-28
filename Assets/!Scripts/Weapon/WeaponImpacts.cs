using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponImpacts : MonoBehaviour
{
    [System.Serializable]
    public class Impact
    {
        public string name;
        public GameObject prefab;
    }

    [Header("Settings")]
    [SerializeField] private Impact[] impacts;

    public Impact GetImpactWithTag(string tag)
    {
        foreach (Impact imp in impacts)
        {
            if (imp.name == tag)
            {
                return imp;
            }
        }

        return null;
    }
}
