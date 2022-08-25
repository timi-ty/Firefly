using Firefly.Core;
using Firefly.Core.Math;
using Firefly.Game.Camera;
using UnityEngine;

namespace Firefly.Game
{
    public class Projectile : PoolableGameObject
    {
        private Vector3 _direction;
        private float _speed;

        public void Fire(Vector3 origin, Vector3 direction, float speed)
        {
            var pTransform = transform;
            pTransform.position = origin;
            _direction = direction.X0Z();
            pTransform.forward = _direction;
            
            _speed = speed;
        }

        private void Update()
        {
            transform.position += _direction * (_speed * Time.deltaTime);
        }

        protected override void OnActivate()
        {
            _direction = default;
            _speed = 0;
        }

        protected override void OnDeactivate()
        {
            _direction = default;
            _speed = 0;
        }
    }
}