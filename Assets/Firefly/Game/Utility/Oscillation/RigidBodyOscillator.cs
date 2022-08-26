using UnityEngine;

namespace Firefly.Game.Utility
{
    [DefaultExecutionOrder(505)]
    public class RigidBodyOscillator : Oscillator
    {
        #region Component References

        private Rigidbody Rigidbody { get; set; }

        #endregion

        protected override Vector3 Position
        {
            get => Rigidbody.position;
            set => Rigidbody.MovePosition(value);
        }

        protected override Vector3 EulerAngles
        {
            get => Rigidbody.rotation.eulerAngles;
            set => Rigidbody.rotation = Quaternion.Euler(value);
        }

        protected override void OnAwaken()
        {
            Rigidbody = GetComponent<Rigidbody>();
            if (!Rigidbody) Rigidbody = GetComponentInChildren<Rigidbody>();
            if (!Rigidbody) Rigidbody = GetComponentInParent<Rigidbody>();

            if (!Rigidbody)
            {
                Debug.LogError($"{this} could not find any RigidBody component in its hierarchy");
            }
        }

        private void FixedUpdate()
        {
            Oscillate();
        }
    }
}