using Firefly.Core;
using UnityEngine;

namespace Firefly.Game
{
    public class Wand : BaseBehaviour
    {
        [SerializeField] 
        private GameObjectPool<Projectile> _projectilePool;

        [SerializeField] 
        private Transform _projectileOrigin;

        protected override void OnAwaken()
        {
            GameObject wandProjectileHolder = new GameObject("WandProjectileHolder");
            _projectilePool.Create(wandProjectileHolder.transform);
        }

        public void Wave()
        {
            var position = _projectileOrigin.position;
            Projectile projectile = _projectilePool.Acquire(position);
            projectile.Fire(position, _projectileOrigin.forward, 5);
        }
    }
}
