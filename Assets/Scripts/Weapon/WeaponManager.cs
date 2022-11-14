using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapon
{
    public class WeaponManager : Singleton<WeaponManager>
    {
        [Header("Settings")]
        public LayerMask hittableMask;
        public Weapon currentWeapon;
        public GameObject tracerPrefab;

        public static LayerMask GetHittableMask
        {
            get
            {
                return Instance.hittableMask;
            }
        }

        public static Weapon CurrentWeapon
        {
            get
            {
                return Instance.currentWeapon;
            }

            set
            {
                Instance.currentWeapon = value;
            }
        }

        public static GameObject GetTracerPrefab
        {
            get
            {
                return Instance.tracerPrefab;
            }
        }
    }
}