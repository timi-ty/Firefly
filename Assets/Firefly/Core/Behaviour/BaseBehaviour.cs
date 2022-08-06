using UnityEngine;

namespace Firefly.Core.Behaviour
{
    public abstract class BaseBehaviour : MonoBehaviour
    {
        public void Awaken()
        {
            int childCount = transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                BaseBehaviour child = transform.GetChild(i).GetComponent<BaseBehaviour>();
                if (child)
                {
                    child.Awaken();
                }
            }
            
            OnAwaken();
        }

        protected virtual void OnAwaken(){}
    }
}
