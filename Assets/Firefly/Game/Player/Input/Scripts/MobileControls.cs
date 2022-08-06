using System;
using Firefly.Core.Behaviour;
using Firefly.Game.Management;
using UnityEngine;

namespace Firefly.Game.Player.Input
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