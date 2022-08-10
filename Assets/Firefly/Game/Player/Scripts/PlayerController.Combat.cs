using UnityEngine;
using UnityEngine.InputSystem;

namespace Firefly.Game
{
    public partial class PlayerController
    {
        [SerializeField] 
        private Wand _wand;

        private bool _fireInput;

        private void CombatUpdate()
        {
            if (_fireInput) _wand.Wave();
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