using Firefly.Core;

namespace Firefly.Game
{
    public class Drone : BaseBehaviour, IHealth
    {
        public Health Health { private set; get; }
    }
}