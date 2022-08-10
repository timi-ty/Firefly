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

        private OscillationConfig _defaultOscillationConfig;

        #endregion

        protected override void OnAwaken()
        {
            _defaultOscillationConfig = _oscillationConfig;
        }

        private void LateUpdate()
        {
            _oscillationFrequency = Mathf.MoveTowards(_oscillationFrequency, _oscillationConfig._targetOscillationFrequency,
                _oscillationConfig._oscillationAgility * Time.deltaTime);
            _oscillationMagnitude = Mathf.MoveTowards(_oscillationMagnitude, _oscillationConfig._targetOscillationMagnitude,
                _oscillationConfig._oscillationAgility * Time.deltaTime);
            
            float angle = 2 * Mathf.PI * _oscillationFrequency * Time.time;
            float normalizedMagnitude = Mathf.Sin(angle);

            if (_oscillationConfig._randomizePosition &&_wasNegative && normalizedMagnitude >= 0)
            {
                _randomizedPositionScale = Vector3.Scale(Random.onUnitSphere, _oscillationConfig._positionScale);
            }

            Vector3 positionScale = _oscillationConfig._randomizePosition ? _randomizedPositionScale : _oscillationConfig._positionScale;
            Vector3 positionOffset = positionScale * (normalizedMagnitude * _oscillationMagnitude);
            Vector3 rotationOffset = _oscillationConfig._rotationScale * (normalizedMagnitude * _oscillationMagnitude);
            
            Transform scopedTransform = transform;
            scopedTransform.position += positionOffset - _lastAppliedPositionOffset;
            scopedTransform.eulerAngles += rotationOffset - _lastAppliedRotationOffset;

            _lastAppliedPositionOffset = positionOffset;
            _lastAppliedRotationOffset = rotationOffset;
            _wasNegative = normalizedMagnitude < 0;
        }

        public void SetOscillationConfig(OscillationConfig newConfig)
        {
            _oscillationConfig = _defaultOscillationConfig = newConfig;
        }

        public void ModifyOscillation(OscillationConfig newConfig, float resetAfter = 1)
        {
            _oscillationConfig = newConfig;
            
            PostAction(ResetOscillationConfig, resetAfter);
        }

        private void ResetOscillationConfig()
        {
            _oscillationConfig = _defaultOscillationConfig;
        }
    }
}