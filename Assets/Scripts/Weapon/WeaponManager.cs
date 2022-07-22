using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [System.Serializable]
    public class Impact
    {
        public string name;
        public GameObject prefab;
    }

    [Header("[Weapons Settings]")]
    public Impact[] Impacts;

    public Impact GetImpactWithTag(string _tag)
    {
        foreach (Impact imp in Impacts)
        {
            if (imp.name == _tag)
            {
                return imp;
            }
        }

        return null;
    }
}