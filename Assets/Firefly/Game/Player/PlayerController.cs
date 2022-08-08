using Firefly.Core;
using Firefly.Core.Math;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace Firefly.Game
{
    public partial class PlayerController : BaseBehaviour
    {
        #region Movement Settings

        [SerializeField, Header("Movement Settings")]
        private float _rotationAgility = 1.0f;
        [SerializeField]
        private float _directionAgility = 1.0f;
        [SerializeField]
        private float _maxSpeed = 1.0f;
        [SerializeField]
        private float _maxAcceleration = 1.0f;
        [SerializeField]
        private float _maxDeceleration = 1.0f;
        [SerializeField, Tooltip("Normalized Curve")] 
        private AnimationCurve _accelerationVsSpeed;
        [SerializeField, Tooltip("Normalized Curve")] 
        private AnimationCurve _decelerationVsSpeed;

        #endregion

        #region Components

        private Rigidbody Rigidbody { get; set; }

        #endregion

        #region Worker Parameters

        private float _currentRotation;
        private float _targetRotation;
    
        private Vector2 _velocity;
        private Vector2 _currentDirection;
        private Vector2 _targetDirection;
        private float _currentSpeed;
        private float _targetSpeed;

        #endregion

        #region Unity Runtime

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            MovementParameterUpdate(Time.deltaTime);
            
            CombatUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            MovementPhysicsUpdate(Time.fixedDeltaTime);
        }

        #endregion

        #region Movement Update

        private void MovementParameterUpdate(float deltaTime)
        {
            _currentRotation = Mathf.MoveTowardsAngle(_currentRotation, _targetRotation,
                MonoMath.Angle(_currentRotation, _targetRotation) * _rotationAgility * deltaTime);

            float acceleration;
            float normalizedSpeed = _currentSpeed / _maxSpeed;
            if (_targetSpeed > _currentSpeed)
            {
                acceleration = _accelerationVsSpeed.Evaluate(normalizedSpeed) * _maxAcceleration;
            }
            else
            {
                acceleration = _decelerationVsSpeed.Evaluate(normalizedSpeed) * _maxDeceleration;
            }

            float deltaSpeed = acceleration * deltaTime;
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _targetSpeed, deltaSpeed);
            _currentDirection = Vector2.MoveTowards(_currentDirection, _targetDirection,
                Vector2.Distance(_currentDirection, _targetDirection) * _directionAgility * deltaTime);
            
            _velocity = _currentDirection * _currentSpeed;
        }

        private void MovementPhysicsUpdate(float deltaTime)
        {
            Rigidbody.rotation = Quaternion.Euler(0, _currentRotation, 0);

            Vector2 nextPosition = _velocity * deltaTime;
            Rigidbody.position += new Vector3(nextPosition.x, 0, nextPosition.y);
        }

        #endregion

        #region Input Receivers

        public void MoveInput(InputAction.CallbackContext context)
        {
            var moveInput = context.ReadValue<Vector2>();

            _targetDirection = moveInput.normalized;
            _targetSpeed = moveInput.magnitude * _maxSpeed;
        }

        public void LookInput(InputAction.CallbackContext context)
        {
            var lookInput = context.ReadValue<Vector2>();

            if (lookInput.sqrMagnitude < 0.1f) return;

            _targetRotation = Vector2.SignedAngle(lookInput, Vector2.up);
        }
        
        #endregion
    }
}