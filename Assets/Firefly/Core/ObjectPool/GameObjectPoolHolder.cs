using Firefly.Game;
using UnityEngine;

namespace Firefly.Core
{
    public abstract class GameObjectPoolHolder<T> : BaseBehaviour where T : PoolableGameObject
    {
        [SerializeField] 
        private GameObjectPool<T> _gameObjectPool;

        protected sealed override void OnAwaken()
        {
            _gameObjectPool.Create(transform);
            OnPoolCreated();
        }

        protected virtual void OnPoolCreated() {}

        public T Acquire()
        {
            return _gameObjectPool.Acquire();
        }
    }
}