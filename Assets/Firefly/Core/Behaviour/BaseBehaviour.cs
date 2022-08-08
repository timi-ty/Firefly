using UnityEngine;

namespace Firefly.Core
{
    public abstract class BaseBehaviour : MonoBehaviour
    {
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

        protected virtual void OnAwaken(){}
    }
}
