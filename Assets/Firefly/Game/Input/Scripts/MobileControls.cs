using Firefly.Core;
using Firefly.Game.Management;

namespace Firefly.Game
{
    public class MobileControls : BaseBehaviour
    {
        protected override void OnAwaken()
        {
            StartGameIncident.Instance.Subscribe(OnGameStart, this);
        }

        private void OnGameStart(StartGameIncident.Data startGameIncident)
        {
            gameObject.SetActive(startGameIncident.IsMobile);
        }

        private void OnDestroy()
        {
            StartGameIncident.Instance.Unsubscribe(this);
        }
    }
}