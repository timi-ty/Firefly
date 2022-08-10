using System;
using Firefly.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Firefly.Game.Utility
{
    [Serializable]
    public struct OscillationConfig
    {
        [Header("Oscillation Settings")]
        [SerializeField, Range(0.0f, 25.0f)]
        public float _targetOscillationFrequency;
        [SerializeField, Range(0.0f, 1.0f)]
        public float _targetOscillationMagnitude;
        [SerializeField]
        public float _oscillationAgility;
        [SerializeField]
        public Vector3 _positionScale;
        [SerializeField] 
        public bool _randomizePosition;
        [SerializeField]
        public Vector3 _rotationScale;
        [SerializeField]
        public bool _invertSineWave;

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
    
    [Serializable]
    public struct OscillationImpulse
    {
        [Header("Oscillation Impulse Settings")]
        [SerializeField]
        public float _magnitude;
        [SerializeField]
        public float _growthRate;
        [SerializeField]
        public float _decayRate;
        [SerializeField]
        public Vector3 _positionScale;
        [SerializeField]
        public Vector3 _rotationScale;
        
        public static OscillationImpulse Impulse10()
        {
            OscillationImpulse oscillationImpulse = new()
            {
                _magnitude = 10,
                _growthRate = 100,
                _decayRate = 20,
                _positionScale = Vector3.up,
                _rotationScale = Vector3.right
            };

            return oscillationImpulse;
        }
    }
    
    public class Oscillator : BaseBehaviour
    {
        #region Settings

        [SerializeField] 
        private OscillationConfig _oscillationConfig = OscillationConfig.Default();

        #endregion

        #region Worker Fields

        private Vector3 _startPosition;
        private Vector3 _startRotation;
        private Vector3 _randomizedPositionScale;
        private float _oscillationFrequency;
        private float _oscillationMagnitude;
        private Vector3 _lastAppliedPositionOffset;
        private Vector3 _lastAppliedRotationOffset;
        private bool _wasNegative;

        private OscillationImpulse _oscillationImpulse;
        private float _impulseMagnitude;
        private float _decayTime;
        
        #endregion

        private void LateUpdate()
        {
            bool isDecaying = Time.time >= _decayTime;
            float impulseTarget = isDecaying ? 0 : _oscillationImpulse._magnitude;
            float impulseDelta = (isDecaying ? _oscillationImpulse._decayRate : _oscillationImpulse._growthRate) * Time.deltaTime;
            _impulseMagnitude = Mathf.MoveTowards(_impulseMagnitude, impulseTarget, impulseDelta);
            
            _oscillationFrequency = Mathf.MoveTowards(_oscillationFrequency, _oscillationConfig._targetOscillationFrequency,
                _oscillationConfig._oscillationAgility * Time.deltaTime);
            _oscillationMagnitude = Mathf.MoveTowards(_oscillationMagnitude, _oscillationConfig._targetOscillationMagnitude, 
                _oscillationConfig._oscillationAgility * Time.deltaTime);
            
            float angle = 2 * Mathf.PI * _oscillationFrequency * Time.time;
            float normalizedMagnitude = _oscillationConfig._invertSineWave ? Mathf.Abs(Mathf.Sin(angle)) : Mathf.Sin(angle);

            if (_oscillationConfig._randomizePosition &&_wasNegative && normalizedMagnitude >= 0)
            {
                _randomizedPositionScale = Vector3.Scale(Random.onUnitSphere, _oscillationConfig._positionScale);
            }

            Vector3 positionScale = _oscillationConfig._randomizePosition ? _randomizedPositionScale : _oscillationConfig._positionScale;
            Vector3 positionOffset = positionScale * (normalizedMagnitude * _oscillationMagnitude) 
                                     + (_impulseMagnitude * _oscillationImpulse._positionScale);
            Vector3 rotationOffset = _oscillationConfig._rotationScale * (normalizedMagnitude * _oscillationMagnitude) 
                                     + (_impulseMagnitude * _oscillationImpulse._rotationScale);
            
            Transform scopedTransform = transform;
            scopedTransform.position += positionOffset - _lastAppliedPositionOffset;
            scopedTransform.eulerAngles += rotationOffset - _lastAppliedRotationOffset;

            _lastAppliedPositionOffset = positionOffset;
            _lastAppliedRotationOffset = rotationOffset;
            _wasNegative = normalizedMagnitude < 0;
        }

        public void SetOscillationConfig(OscillationConfig newConfig)
        {
            _oscillationConfig = newConfig;
        }

        public void Impulse(OscillationImpulse oscillationImpulse)
        {
            float impulseDiff = oscillationImpulse._magnitude - _impulseMagnitude;
            
            if (impulseDiff > 0)
            {
                _oscillationImpulse = oscillationImpulse;
                _decayTime = Time.time + impulseDiff / oscillationImpulse._growthRate;
            }
        }
    }
}