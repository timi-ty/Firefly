using System;
using Firefly.Core;
using UnityEngine;

namespace Firefly.Game.Management
{
    public class GameManager : BaseBehaviour
    {
        [SerializeField] private bool _isMobile;

        private void Start()
        {
            StartGameIncident.Instance.Publish(new StartGameIncident.Data(_isMobile));
        }
    }
}
