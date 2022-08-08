using System.Collections.Generic;
using Firefly.Core;
using UnityEngine;

namespace Firefly.Game.Camera
{
    public enum VibrationLevel { Light, Medium, Heavy }
    
    public class CameraDirector : BaseBehaviour
    {
        private const float SizeFactor = 1.333333333f;

        public UnityEngine.Camera _sceneCamera;
        private static CameraDirector _cameraDirector;

        private readonly List<Vector2> _destPoints = new();
        private readonly Vector2[] _currentPath = new Vector2[3];

        private float[] _defaultVibration;

        private Vector2 _deviationFromEquilibrium;
        
        [Header("Vibration Settings")]
        [Min(3)]
        public int _destPointCount;
        [Range(0.0f, 25.0f)]
        public float _vibrationFrequency;
        [Range(0.0f, 1.0f)]
        public float _restVibrationMagnitude = 0.025f;
        [Range(0.0f, 1.0f)]
        public float _lightVibrationMagnitude = 0.035f;
        [Range(0.0f, 1.0f)]
        public float _mediumVibrationMagnitude = 0.055f;
        [Range(0.0f, 1.0f)]
        public float _heavyVibrationMagnitude = 0.075f;

        void OnEnable()
        {
            int perfectAspectRatio = 2;
            float deviation = perfectAspectRatio - _sceneCamera.aspect;
            float deltaSize = deviation * SizeFactor;
            _sceneCamera.orthographicSize = 5 + deltaSize;

            _cameraDirector = this;
            
            GenerateDestPoints();

            _defaultVibration = new[] { _vibrationFrequency, _restVibrationMagnitude };
        }

        void Update()
        {
            var cTransform = _sceneCamera.transform;
            Vector3 planarDeviation = cTransform.right * _deviationFromEquilibrium.x +
                                      cTransform.up * _deviationFromEquilibrium.y;

            var position = cTransform.position;
            _sceneCamera.transform.position = Vector3.MoveTowards(position, 
                position + planarDeviation, 10 * Time.deltaTime);

            VibrateCamera();
        }

        private void VibrateCamera()
        {
            //vibrationMagnitude is the distance applied between any two destPoints. Thus, multiply by destPoint.Count to get total distance for one cycle.
            float speed = _vibrationFrequency * _restVibrationMagnitude * _destPoints.Count;
            Vector2 direction = _currentPath[2];
            Vector2 deltaPos = direction * (speed * Time.fixedDeltaTime);
            _deviationFromEquilibrium += deltaPos;

            Vector2 startPoint = _currentPath[0];
            Vector2 endPoint = _currentPath[1];
            Vector2 currentPoint = _deviationFromEquilibrium;

            if((currentPoint - startPoint).sqrMagnitude >= (endPoint - startPoint).sqrMagnitude)
            {
                startPoint = currentPoint;
                endPoint = _destPoints[Random.Range(0, _destPoints.Count)] * _restVibrationMagnitude;

                _currentPath[0] = startPoint;
                _currentPath[1] = endPoint;
                _currentPath[2] = (endPoint - startPoint).normalized;
            }
        }

        private void GenerateDestPoints()
        {
            _destPoints.Add(Vector2.zero);
            for(int i = 1; i < _destPointCount; i++)
            {
                _destPoints.Add(Random.insideUnitCircle);
            }

            _currentPath[0] = _destPoints[0] * _restVibrationMagnitude;
            _currentPath[1] = _destPoints[1] * _restVibrationMagnitude;
            _currentPath[2] = (_destPoints[1] - _destPoints[0]).normalized;

            _deviationFromEquilibrium = _currentPath[0];
        }

        public static void CreateVibration(VibrationLevel vibration, float t)
        {
            switch (vibration)
            {
                case VibrationLevel.Light:
                    _cameraDirector._restVibrationMagnitude = _cameraDirector._lightVibrationMagnitude;
                    break;
                case VibrationLevel.Medium:
                    _cameraDirector._restVibrationMagnitude = _cameraDirector._mediumVibrationMagnitude;
                    break;
                case VibrationLevel.Heavy:
                    _cameraDirector._restVibrationMagnitude = _cameraDirector._heavyVibrationMagnitude;
                    break;
            }

            _cameraDirector.Invoke(nameof(ResetToRestVibration), t);
        }

        private void ResetToRestVibration()
        {
            _vibrationFrequency = _defaultVibration[0];
            _restVibrationMagnitude = _defaultVibration[1];
        }

        [ContextMenu("Heavy Vibration")]
        public void CreateHeavyVibration()
        {
            CreateVibration(VibrationLevel.Heavy, 1);
        }
    }
}