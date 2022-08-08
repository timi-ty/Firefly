using Firefly.Core;
using Firefly.Game.Management;

namespace Firefly.Game
{
    public class MobileControls : BaseBehaviour
    {
        private void Start()
        {
            StartGameIncident.Subscribe(OnGameStart, this);
        }

        private void OnGameStart(StartGameIncident startGameIncident)
        {
            if (!startGameIncident.IsMobile) gameObject.SetActive(false);
        }
    }
}