using System;
using Firefly.Core;
using Firefly.Core.Incident;

namespace Firefly.Game.Management
{
    public class StartGameIncident : BaseIncident<StartGameIncident>
    {
        public static StartGameIncident Instance { get; } = new();
        protected override StartGameIncident IncidentInstance => Instance;
        public bool IsMobile { get; private set; }
        
        private StartGameIncident(){}

        /// <summary>
        /// A BaseBehaviour can have only one subscription to an incident. This method throws an error if there is already a subscription on this context.
        /// </summary>
        /// <param name="subscription">The handler for this incident.</param>
        /// <param name="context">The unique context holding the incident handler.</param>
        /// <param name="publishPending">Controls whether this subscription publishes immediately with the current state of the incident.</param>
        public static void Subscribe(Action<StartGameIncident> subscription, BaseBehaviour context, bool publishPending = true)
        {
            if (publishPending && Instance.HasBeenPublished) subscription?.Invoke(Instance);
            
            Instance.BaseSubscribe(subscription, context);
        }

        public static void PublishWith(bool isMobile)
        {
            Instance.IsMobile = isMobile;
            Instance.Publish();
        }

        public static void Unsubscribe(BaseBehaviour context) => Instance.BaseUnsubscribe(context);
    }
}
