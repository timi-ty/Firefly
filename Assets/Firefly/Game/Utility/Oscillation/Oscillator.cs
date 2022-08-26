using System;
using Firefly.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Firefly.Game.Utility
{
    [Serializable]
    public struct OscillationImpulse
    {
        #region Private Serialized Fields

        [Header("Oscillation Impulse Settings")]
        [SerializeField]
        private float _magnitude;
        [SerializeField]
        private float _growthRate;
        [SerializeField]
        private float _decayRate;
        [SerializeField]
        private Vector3 _positionScale;
        [SerializeField]
        private Vector3 _rotationScale;

        #endregion

        #region Public Accessors

        public float Magnitude => _magnitude;

        public float GrowthRate => _growthRate;

        public float DecayRate => _decayRate;

        public Vector3 PositionScale => _positionScale;

        public Vector3 RotationScale => _rotationScale;

        #endregion

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
    
    public abstract class Oscillator : BaseBehaviour, IOscillator
    {
        #region Settings

        [SerializeField] 
        private OscillationConfigData _oscillationConfigData;
        private OscillationConfig OscillationConfig => _oscillationConfigData.OscillationConfig;

        #endregion

        #region Abstract Properties

        protected abstract Vector3 Position { get; set; }
        protected abstract Vector3 EulerAngles { get; set; }

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

        protected void Oscillate()
        {
            bool isDecaying = Time.time >= _decayTime;
            float impulseTarget = isDecaying ? 0 : _oscillationImpulse.Magnitude;
            float impulseDelta = (isDecaying ? _oscillationImpulse.DecayRate : _oscillationImpulse.GrowthRate) * Time.deltaTime;
            _impulseMagnitude = Mathf.MoveTowards(_impulseMagnitude, impulseTarget, impulseDelta);
            
            _oscillationFrequency = Mathf.MoveTowards(_oscillationFrequency, OscillationConfig.TargetOscillationFrequency,
                OscillationConfig.OscillationAgility * Time.deltaTime);
            _oscillationMagnitude = Mathf.MoveTowards(_oscillationMagnitude, OscillationConfig.TargetOscillationMagnitude, 
                OscillationConfig.OscillationAgility * Time.deltaTime);
            
            float angle = 2 * Mathf.PI * _oscillationFrequency * Time.time;
            float normalizedMagnitude = OscillationConfig.InvertSineWave ? Mathf.Abs(Mathf.Sin(angle)) : Mathf.Sin(angle);

            if (OscillationConfig.RandomizePosition && _wasNegative && normalizedMagnitude >= 0)
            {
                _randomizedPositionScale = Vector3.Scale(Random.onUnitSphere, OscillationConfig.PositionScale);
            }

            Vector3 positionScale = OscillationConfig.RandomizePosition ? _randomizedPositionScale : OscillationConfig.PositionScale;
            Vector3 positionOffset = positionScale * (normalizedMagnitude * _oscillationMagnitude) 
                                     + (_impulseMagnitude * _oscillationImpulse.PositionScale);
            Vector3 rotationOffset = OscillationConfig.RotationScale * (normalizedMagnitude * _oscillationMagnitude) 
                                     + (_impulseMagnitude * _oscillationImpulse.RotationScale);

            Position += positionOffset - _lastAppliedPositionOffset;
            EulerAngles += rotationOffset - _lastAppliedRotationOffset;

            _lastAppliedPositionOffset = positionOffset;
            _lastAppliedRotationOffset = rotationOffset;
            _wasNegative = normalizedMagnitude < 0;
        }

        public void SetOscillationConfig(OscillationConfig newConfig)
        {
            _oscillationConfigData.SetRuntimeOscillation(newConfig);
        }

        public void Impulse(OscillationImpulse oscillationImpulse)
        {
            float impulseDiff = oscillationImpulse.Magnitude - _impulseMagnitude;
            
            if (impulseDiff > 0)
            {
                _oscillationImpulse = oscillationImpulse;
                _decayTime = Time.time + impulseDiff / oscillationImpulse.GrowthRate;
            }
        }
    }
}