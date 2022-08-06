using System;
using Firefly.Core.Behaviour;
using Firefly.Core.Incident;
using UnityEngine;

namespace Firefly.Game.Management
{
    public class StartGameIncident : BaseIncident<StartGameIncident>
    {
        public static StartGameIncident Instance { get; } = new();
        protected override StartGameIncident IncidentInstance => Instance;
        public bool IsMobile { get; private set; }
        
        private StartGameIncident(){}

        public static void Subscribe(Action<StartGameIncident> subscription, BaseBehaviour context) =>
            Instance.BaseSubscribe(subscription, context);
        public static void PublishWith(bool isMobile)
        {
            Instance.IsMobile = isMobile;
            Instance.Publish();
        }
    }
}
