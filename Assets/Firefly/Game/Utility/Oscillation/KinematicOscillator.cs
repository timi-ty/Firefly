using System;
using UnityEngine;

namespace Firefly.Game.Utility
{
    [DefaultExecutionOrder(505)]
    public class KinematicOscillator : Oscillator
    {
        #region Serialized Refernces

        [SerializeReference] 
        private Transform _transform;

        #endregion

        private Transform Transform => _transform ? _transform : transform;
        
        protected override Vector3 Position
        {
            get => Transform.position;
            set => Transform.position = value;
        }

        protected override Vector3 EulerAngles
        {
            get => Transform.eulerAngles;
            set => Transform.eulerAngles = value;
        }

        private void LateUpdate()
        {
            Oscillate();
        }
    }
}