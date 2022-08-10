using Firefly.Core;
using Firefly.Game.Utility;
using UnityEngine;

namespace Firefly.Game.Camera
{
    public class CameraDirector : SingletonBehaviour<CameraDirector>
    {
        [SerializeField] 
        private Oscillator _oscillator;

        [SerializeField] 
        private float _heavyVibrationMagnitude;

        public static void VibrateHeavy(float duration)
        {
            OscillationConfig heavyOscillation = OscillationConfig.Default();
            heavyOscillation._targetOscillationMagnitude = Instance._heavyVibrationMagnitude;
            heavyOscillation._randomizePosition = false;
            heavyOscillation._rotationScale = Vector3.right;

            Instance._oscillator.ModifyOscillation(heavyOscillation, duration);
        }
    }
}