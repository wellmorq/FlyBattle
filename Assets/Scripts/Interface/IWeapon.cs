namespace FlyBattle.Interface
{
    public interface IWeapon
    {
        bool GetReload { get; }
        void OnShoot();
        void SetShootActive(bool flag);
    }
}