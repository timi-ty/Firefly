using UnityEngine;

namespace Firefly.Core
{
    public abstract class PoolableGameObject : BaseBehaviour
    {
        #region Serialized Fields

        [Header("Poolable Object")][SerializeField]
        private bool _ignoreLifetime;
        [SerializeField]
        private float _lifeTime;

        #endregion

        #region Properties

        public bool IsActive { get; private set; }
        protected bool IgnoreLifetime
        {
            get => _ignoreLifetime;
            set => _ignoreLifetime = value;
        }

        #endregion

        #region Worker Fields

        private float _remainingLifeTime;

        #endregion

        private void Update()
        {
            if(!IgnoreLifetime && _remainingLifeTime <= 0)
            {
                Deactivate();
                return;
            }

            _remainingLifeTime -= Time.deltaTime;
            
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {
            
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
