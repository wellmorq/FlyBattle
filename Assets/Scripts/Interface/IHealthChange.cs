namespace FlyBattle.Interface
{
    public interface IHealthChange
    {
        void Damage(IHealth me);
        float DamageCount { get; }
    }
}