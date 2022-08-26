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

        #region Serialized Fields

        [Header("Settings")] [SerializeField] 
        private float _projectileSpeed = 25;
        [SerializeField]
        private float _fireCooldown = 0.2f;

        #endregion

        #region Worker Fields

        private float _fireCooldownTime;

        #endregion

        protected override void OnAwaken()
        {
            GameObject wandProjectileHolder = new GameObject("WandProjectileHolder");
            _projectilePool.Create(wandProjectileHolder.transform);
            _fireCooldownTime = _fireCooldown;
        }

        private void Update()
        {
            _fireCooldownTime -= _fireCooldownTime > 0 ? Time.deltaTime : 0;
        }

        public void Wave()
        {
            if(_fireCooldownTime > 0) return;
            
            var position = _projectileOrigin.position;
            Projectile projectile = _projectilePool.Acquire(position);
            projectile.Fire(position, _projectileOrigin.forward, _projectileSpeed);
            
            CameraDirector.VibrateHeavy();
            
            _fireCooldownTime = _fireCooldown;
        }
    }
}
