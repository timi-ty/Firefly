using System;
using Firefly.Core.Application;
using UnityEngine;

namespace Firefly.Game.Utility
{
    [Serializable]
    public struct OscillationConfig
    {
        #region Private Serialized Fields

        [Header("Oscillation Settings")]
        [SerializeField, Range(0.0f, 25.0f)]
        private float _targetOscillationFrequency;
        [SerializeField, Range(0.0f, 1.0f)] 
        private float _targetOscillationMagnitude;
        [SerializeField] 
        private float _oscillationAgility;
        [SerializeField] 
        private Vector3 _positionScale;
        [SerializeField] 
        private bool _randomizePosition;
        [SerializeField] 
        private Vector3 _rotationScale;
        [SerializeField] 
        private bool _invertSineWave;

        #endregion
        
        #region Public Accessors

        public float TargetOscillationFrequency => _targetOscillationFrequency;
        public float TargetOscillationMagnitude => _targetOscillationMagnitude;
        public float OscillationAgility => _oscillationAgility;
        public Vector3 PositionScale => _positionScale;
        public bool RandomizePosition => _randomizePosition;
        public Vector3 RotationScale => _rotationScale;
        public bool InvertSineWave => _invertSineWave;

        #endregion
        
        public static OscillationConfig Default()
        {
            OscillationConfig oscillationConfig = new()
            {
                _targetOscillationFrequency = 10,
                _targetOscillationMagnitude = 0.025f,
                _oscillationAgility = 10,
                _positionScale = Vector3.one,
                _randomizePosition = true,
                _rotationScale = Vector3.one
            };

            return oscillationConfig;
        }
    }
    
    [CreateAssetMenu(fileName = "OscillationConfig", menuName = MenuPath)]
    public class OscillationConfigData : ScriptableObject
    {
        private const string MenuPath = FireflyApp.AppName + "/Oscillation/Config";
        
        [SerializeField] 
        private OscillationConfig _oscillationConfig = OscillationConfig.Default();
        
        private bool _hasRuntimeOscillationConfig;
        private OscillationConfig _runtimeOscillationConfig = OscillationConfig.Default();
        
        public OscillationConfig OscillationConfig => _hasRuntimeOscillationConfig ? _runtimeOscillationConfig : _oscillationConfig;

        public void SetRuntimeOscillation(OscillationConfig runtimeOscillationConfig)
        {
            _hasRuntimeOscillationConfig = true;
            _runtimeOscillationConfig = runtimeOscillationConfig;
        }

        public void IgnoreRuntimeOscillationConfig()
        {
            if (_hasRuntimeOscillationConfig)
            {
                _hasRuntimeOscillationConfig = false;
            }
            else
            {
                Debug.LogWarning("Tried to ignore runtime oscillation but there is none");
            }
        }
    }
}