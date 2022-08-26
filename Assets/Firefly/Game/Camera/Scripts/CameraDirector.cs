using Firefly.Core;
using Firefly.Game.Utility;
using UnityEngine;

namespace Firefly.Game.Camera
{
    public class CameraDirector : SingletonBehaviour<CameraDirector>
    {
        #region Private Serialized Fields

        [SerializeReference]
        private Oscillator _oscillator;
        [SerializeField] 
        private OscillationImpulse _heavyOscillationImpulse = OscillationImpulse.Impulse10();

        #endregion

        #region Accessor Properties

        private IOscillator Oscillator => _oscillator;

        #endregion

        public static void VibrateHeavy()
        {
            Instance.Oscillator.Impulse(Instance._heavyOscillationImpulse);
        }
    }
}