using Code.Player;

namespace Code.Interfaces
{
    public interface IDamageable<T>
    {
        void TakeDamage(T damage);
    }
}