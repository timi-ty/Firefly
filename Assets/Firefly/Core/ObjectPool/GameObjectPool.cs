using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Firefly.Core
{
    [Serializable]
    public class GameObjectPool<T> where T : PoolableGameObject
    {
        [SerializeReference] 
        private T _objectPrefab;
        
        [SerializeField] 
        private int _size;

        private Queue<T> _objectQueue;

        public void Create(Transform holder, int size = -1)
        {
            if (_objectQueue != null)
            {
                Debug.LogError($"{nameof(GameObjectPool<T>)}:::This pool has already been created");
                return;
            }

            if (size < 0) size = _size;

            _objectQueue = new Queue<T>(size);

            if (!_objectPrefab)
            {
                Debug.Log($"Created empty pool of {nameof(T)}");
                return;
            }

            for (int i = 0; i < size; i++)
            {
                T poolableObject = Object.Instantiate(_objectPrefab, holder);
                poolableObject.name = $"{poolableObject.name} {i}";
                poolableObject.gameObject.SetActive(false);
                _objectQueue.Enqueue(poolableObject);
            }
            
            Debug.Log($"Created pool of {_objectPrefab.name} | Size {size}");
        }

        public T Acquire(Vector3 position = default)
        {
            T poolableObject = _objectQueue.Dequeue();
            _objectQueue.Enqueue(poolableObject);

            if (poolableObject.IsActive)
            {
                Debug.LogWarning($"{nameof(GameObjectPool<T>)}:::This pool is empty | Recycling an active {poolableObject} pool object | This may cause unintended behaviour");
                poolableObject.transform.position = position;
                poolableObject.Deactivate();
                poolableObject.Activate();
                return poolableObject;
            }

            poolableObject.transform.position = position;
            poolableObject.Activate();
            return poolableObject;
        }
    }
}