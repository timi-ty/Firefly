using Firefly.Core.Incident;

namespace Firefly.Game.Management
{
    public class StartGameIncident : Incident<StartGameIncident.Data>
    {
        public static StartGameIncident Instance { get; } = new();
        private StartGameIncident(){}
        
        public readonly struct Data
        {
            public bool IsMobile { get; }

            public Data(bool isMobile)
            {
                IsMobile = isMobile;
            }
        }
    }
}