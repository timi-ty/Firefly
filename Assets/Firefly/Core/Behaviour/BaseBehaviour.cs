using System;
using System.Collections;
using UnityEngine;

namespace Firefly.Core
{
    public abstract class BaseBehaviour : MonoBehaviour, IBaseBehaviour
    {
        #region Stubbed Out Unity Runtime

        private void Awake()
        {
            //Do nothing
        }

        #endregion

        public string Name => name;

        public void Awaken()
        {
            AwakenChildren(transform);
            
            OnAwaken();
        }

        private void AwakenChildren(Transform parentTransform)
        {
            int childCount = parentTransform.childCount;

            if (childCount <= 0) return;

            for (int i = 0; i < childCount; i++)
            {
                Transform childTransform = parentTransform.GetChild(i);
                BaseBehaviour childBehaviour = childTransform.GetComponent<BaseBehaviour>();
                
                if (childBehaviour)
                {
                    childBehaviour.Awaken();
                }
                else
                {
                    AwakenChildren(childTransform);
                }
            }
        }

        protected void PostAction(Action action, float delayTime)
        {
            StartCoroutine(UtilityCoroutine(action, delayTime));
        }

        private IEnumerator UtilityCoroutine(Action action, float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            
            action?.Invoke();
        }

        protected virtual void OnAwaken(){}
    }
}
