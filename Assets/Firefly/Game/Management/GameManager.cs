using Firefly.Core;
using UnityEngine;

namespace Firefly.Game.Management
{
    public class GameManager : BaseBehaviour
    {
        [SerializeField] private bool _isMobile;

        protected override void OnAwaken()
        {
            StartGameIncident.PublishWith(_isMobile);
        }
    }
}
