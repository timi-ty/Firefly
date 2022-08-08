using UnityEngine;

namespace Firefly.Core
{
    public abstract class PoolableGameObject<T> : MonoBehaviour where T : PoolableGameObject<T>
    {
        private GameObjectPool<T> _pool;

        public void AddToPool(GameObjectPool<T> pool, bool deactivate = true)
        {
            if (_pool != null)
            {
                Debug.LogError($"{nameof(PoolableGameObject<T>)}:::This object is already part of a pool");
                return;
            }
            
            _pool = pool;

            if (deactivate) gameObject.SetActive(false);
            
            _pool.Recycle(Self());
        }
        
        /// <summary>
        /// Marks this poolable object to be recycled. Use in place of destroying the object if it is poolable.
        /// </summary>
        protected void Deactivate()
        {
            gameObject.SetActive(false);
            _pool.Recycle(Self());
        }

        /// <summary>
        /// Return the instance of the poolable object here.
        /// </summary>
        /// <returns></returns>
        protected abstract T Self();
    }
}
