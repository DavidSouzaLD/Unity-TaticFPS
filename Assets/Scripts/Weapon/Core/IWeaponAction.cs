namespace WeaponSystem.Core.Actions
{
    public interface IWeaponAction
    {
        public Weapon weapon { get; set; }
        public WeaponSO data { get; set; }
        public bool isActive { get; set; }
        public void Start();
        public void Update();
    }
}