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
        OscillationImpulse _heavyOscillationImpulse = OscillationImpulse.Impulse10();

        public static void VibrateHeavy(float duration)
        {
            Instance._oscillator.Impulse(Instance._heavyOscillationImpulse);
        }
    }
}