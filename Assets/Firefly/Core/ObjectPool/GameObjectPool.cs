using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Firefly.Core
{
    [Serializable]
    public class GameObjectPool<T> where T : PoolableGameObject<T>
    {
        [SerializeReference] 
        private T _objectPrefab;
        
        [SerializeField] 
        private int _size;

        private Queue<T> _inactiveQueue;
        private HashSet<T> _activeSet;

        public void Create(Transform holder, int size = -1)
        {
            if (_inactiveQueue != null)
            {
                Debug.LogError($"{nameof(GameObjectPool<T>)}:::This pool has already been created");
                return;
            }

            if (size < 0) size = _size;

            _inactiveQueue = new Queue<T>(size);
            _activeSet = new HashSet<T>(size);

            if (!_objectPrefab)
            {
                Debug.Log($"Created empty pool of {nameof(T)}");
                return;
            }

            for (int i = 0; i < size; i++)
            {
                T poolObject = Object.Instantiate(_objectPrefab, holder);
                poolObject.name = $"{poolObject.name} {i}";
                _activeSet.Add(poolObject);
                poolObject.AddToPool(this);
            }
            
            Debug.Log($"Created pool of {_objectPrefab.name} | Size {size}");
        }

        public T Acquire(Vector3 position = default)
        {
            if (_inactiveQueue.Count > 0)
            {
                T poolableObject = _inactiveQueue.Dequeue();
                poolableObject.gameObject.SetActive(true);
                poolableObject.transform.position = position;
                _activeSet.Add(poolableObject);
                return poolableObject;
            }

            if (_activeSet.Count > 0)
            {
                //find and return the oldest active poolable object here.
                Debug.LogWarning($"{nameof(GameObjectPool<T>)}:::This pool is empty | Recycling an active pool object | This might cause unintended behaviour");
            }

            Debug.LogError($"{nameof(GameObjectPool<T>)}:::This pool is completely empty");

            return null;
        }

        public void Recycle(T poolObject)
        {
            if (_activeSet.Remove(poolObject))
            {
                _inactiveQueue.Enqueue(poolObject);
            }
            else
            {
                Debug.LogError($"{nameof(GameObjectPool<T>)}:::Tired to recycle {poolObject.name}, but it does not belong to this pool");
            }
        }
    }
}
