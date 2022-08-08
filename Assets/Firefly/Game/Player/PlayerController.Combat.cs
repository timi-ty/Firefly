using UnityEngine;
using UnityEngine.InputSystem;

namespace Firefly.Game
{
    public partial class PlayerController
    {
        [SerializeField] 
        private Wand _wand;

        private bool _fireInput;
        private float _fireCooldown;

        private void CombatUpdate(float deltaTime)
        {
            _fireCooldown -= _fireCooldown > 0 ? deltaTime : 0;
            
            if (_fireInput && _fireCooldown <= 0)
            {
                _fireCooldown = 0.2f;
                _wand.Wave();
            }
        }
        
        #region Input Receivers
        
        public void FireInput(InputAction.CallbackContext context)
        {
            var lookInput = context.ReadValue<Vector2>();
            
            _fireInput = lookInput.sqrMagnitude > 0.1;
        }
        
        #endregion
    }
}