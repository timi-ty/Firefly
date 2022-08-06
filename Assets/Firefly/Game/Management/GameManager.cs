using Firefly.Core.Behaviour;
using UnityEngine;

namespace Firefly.Game.Management
{
    public class GameManager : BaseBehaviour
    {
        public bool isMobile;

        protected override void OnAwaken()
        {
            StartGameIncident.PublishWith(isMobile);
        }
    }
}
