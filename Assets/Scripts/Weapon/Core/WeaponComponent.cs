using UnityEngine;

namespace WeaponSystem.Components
{
    [System.Serializable]
    public abstract class WeaponComponent : MonoBehaviour
    {
        public abstract void Start();
        public abstract void Update();
    }
}