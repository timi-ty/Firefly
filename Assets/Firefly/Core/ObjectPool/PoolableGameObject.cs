using UnityEngine;

namespace Firefly.Core
{
    public abstract class PoolableGameObject : BaseBehaviour
    {
        [Header("Poolable Object")][SerializeField]
        private float _lifeTime;
        
        public bool IsActive { get; private set; }

        private float _remainingLifeTime;
        
        private void Update()
        {
            if(_remainingLifeTime <= 0)
            {
                Deactivate();
                return;
            }

            _remainingLifeTime -= Time.deltaTime;
        }


        public void Activate()
        {
            _remainingLifeTime = _lifeTime;
            IsActive = true;
            gameObject.SetActive(true);
            OnActivate();
        }
        
        /// <summary>
        /// Marks this poolable object to be recycled. Use in place of destroying the poolable object.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
            gameObject.SetActive(false);
            OnDeactivate();
        }

        /// <summary>
        /// A poolable object must define a function that activates it. This function should be written like a constructor.
        /// </summary>
        protected abstract void OnActivate();
        
        /// <summary>
        /// A poolable object must define a function that resets it's behaviour when it gets deactivated. This should be written like a destructor.
        /// </summary>
        protected abstract void OnDeactivate();
    }
}
