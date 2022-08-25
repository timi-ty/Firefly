using Firefly.Core;

namespace Firefly.Game
{
    public class Enemy : BaseBehaviour, IHealth
    {
        public Health Health { private set; get; }
    }
}