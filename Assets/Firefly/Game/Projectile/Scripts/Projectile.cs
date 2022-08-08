using Firefly.Core;
using Firefly.Game.Camera;
using UnityEngine;

namespace Firefly.Game
{
    public class Projectile : PoolableGameObject<Projectile>
    {
        protected override Projectile Self() => this;

        private Vector3 _direction;
        private float _lifeTime;

        public void Fire(Vector3 origin, Vector3 direction, float lifeTime)
        {
            var pTransform = transform;
            pTransform.position = origin;
            pTransform.forward = direction;
            _direction = direction;
            _lifeTime = lifeTime;
            
            CameraDirector.CreateVibration(VibrationLevel.Heavy, 0.1f);
        }

        private void Update()
        {
            if(_lifeTime <= 0)
            {
                Deactivate();
                return;
            }

            _lifeTime -= Time.deltaTime;

            transform.position += _direction * (25 * Time.deltaTime);
        }
    }
}
