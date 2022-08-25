namespace Firefly.Game
{
    public interface IHealth
    {
        public Health Health { get; }
    }
    
    public class Health
    {
        private float _health;
    }
}