using System;
using Firefly.Core;
using Firefly.Game.Camera;
using UnityEngine;

namespace Firefly.Game
{
    public class Wand : BaseBehaviour
    {
        [SerializeField] 
        private GameObjectPool<Projectile> _projectilePool;

        [SerializeField] 
        private Transform _projectileOrigin;
        
        private float _fireCooldown;

        protected override void OnAwaken()
        {
            GameObject wandProjectileHolder = new GameObject("WandProjectileHolder");
            _projectilePool.Create(wandProjectileHolder.transform);
        }

        private void Update()
        {
            _fireCooldown -= _fireCooldown > 0 ? Time.deltaTime : 0;
        }

        public void Wave()
        {
            if(_fireCooldown > 0) return;
            
            var position = _projectileOrigin.position;
            Projectile projectile = _projectilePool.Acquire(position);
            projectile.Fire(position, _projectileOrigin.forward, 5);
            
            CameraDirector.VibrateHeavy(0.15f);
            
            _fireCooldown = 0.2f;
        }
    }
}
