using UnityEngine;
using WeaponSystem.Core.Actions;

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

        [Header("Magazine")]
        public WeaponMagazine currentMagazine;
        public WeaponMagazine[] magazines;

        // Actions
        WeaponAction[] actionList;
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
            actionList = new WeaponAction[3];
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

            // Getting magazines
            magazines = WeaponUtils.CreateMagazines(data.startMagazinesQuantity, data.bulletsPerMagazine);
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