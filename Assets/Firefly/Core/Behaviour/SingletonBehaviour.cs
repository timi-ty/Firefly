using System;
using System.Collections;
using UnityEngine;

namespace Firefly.Core
{
    public abstract class SingletonBehaviour<T> : BaseBehaviour, IBaseBehaviour
    {
        protected static T Instance { get; private set; }
        
        public new void Awaken()
        {
            if (Instance == null)
            {
                Instance = GetComponent<T>();
                OnSingletonAwaken();
            }
            else
            {
                Debug.LogError($"{name} is being destroyed because there is already an instance of {nameof(T)} in this scene");
                Destroy(gameObject);
                return;
            }
            
            base.Awaken();
        }
        
        protected virtual void OnSingletonAwaken(){}
    }
}
