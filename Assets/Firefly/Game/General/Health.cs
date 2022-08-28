namespace Firefly.Game
{
    public interface IHealth
    {
        public Health Health { get; }
    }
    
    public class Health
    {
        private readonly float _maxHealth;
        
        private float _health;

        public Health(float maxHealth)
        {
            _health = maxHealth;
            _maxHealth = maxHealth;
        }

        public void Refill()
        {
            _health = _maxHealth;
        }
    }
}