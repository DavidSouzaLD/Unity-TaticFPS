using UnityEngine;
using WeaponSystem.Core.Actions;
using WeaponSystem.Others;

namespace WeaponSystem.Core
{
    public class Weapon : MonoBehaviour
    {
        // Variables
        [Header("Settings")]
        public WeaponType type;
        public FireMode fireMode;
        public WeaponSO data;
        public Transform fireRoot;
        public GameObject muzzleObject;
        public BulletDropping bulletDropping;

        [Header("Magazine")]
        public int currentBullets;
        public int extraBullets;
        public int bulletsPerMagazine;

        // Actions
        IWeaponAction[] actionList;
        public FireAction fireAction { get; private set; }
        public ReloadAction reloadAction { get; private set; }
        public AimAction aimAction { get; private set; }

        // Status
        public bool isFiring { get { return fireAction.isActive; } }
        public bool isReloading { get { return reloadAction.isActive; } }
        public bool isAiming { get { return aimAction.isActive; } }

        // Events
        public delegate void onFiring();
        public delegate void startReload();
        public delegate void endReload();

        public onFiring OnFiring;
        public startReload StartReload;
        public endReload EndReload;

        private void Awake()
        {
            // Create actions
            fireAction = new FireAction(this, data);
            reloadAction = new ReloadAction(this, data);
            aimAction = new AimAction(this, data);

            // Setting actions
            actionList = new IWeaponAction[3];
            actionList[0] = fireAction;
            actionList[1] = reloadAction;
            actionList[2] = aimAction;
        }

        private void Start()
        {
            // Call start actions
            for (int i = 0; i < actionList.Length; i++)
            {
                actionList[i].Start();
            }
        }

        private void Update()
        {
            // Call update actions
            for (int i = 0; i < actionList.Length; i++)
            {
                actionList[i].Update();
            }
        }
    }
}