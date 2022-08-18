namespace Firefly.Core
{
    public interface IDamageable
    {
        public float Health { get; }

        public void TakeDamage(float damage);
    }
}