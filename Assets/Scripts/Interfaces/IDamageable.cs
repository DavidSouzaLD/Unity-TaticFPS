namespace Game.Interfaces
{
    public interface IDamageable<T>
    {
        void TakeDamage(T _damage);
    }
}