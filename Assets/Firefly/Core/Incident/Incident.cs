using System;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly.Core.Incident
{
    public abstract class Incident<T>  where T : struct
    {
        private readonly Dictionary<BaseBehaviour, Action<T>> _incidentSubscriptions = new();
        
        /// <summary>
        /// A BaseBehaviour can have only one subscription to an incident. This method throws an error if there is already a subscription on this context.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context">The BaseBehaviour holding on to this subscription.</param>
        public void Subscribe(Action<T> action, BaseBehaviour context)
        {
            bool incidentExists = _incidentSubscriptions.ContainsKey(context);
            
            if (incidentExists)
            {
                Debug.LogError($"{context.name} is already subscribed to this incident");
                return;
            }
            
            _incidentSubscriptions.Add(context, action);
        }
        
        /// <summary>
        /// A BaseBehaviour can have only one subscription to an incident. This method returns false if there is already a subscription on this context.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context">The BaseBehaviour holding on to this subscription.</param>
        public bool TrySubscribe(Action<T> action, BaseBehaviour context)
        {
            bool incidentExists = _incidentSubscriptions.ContainsKey(context);
            
            if (incidentExists)
            {
                return false;
            }
            
            _incidentSubscriptions.Add(context, action);

            return true;
        }
        
        /// <summary>
        /// A BaseBehaviour can have only one subscription to an incident. This method will overwrite the current subscription on this context if there is one.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="context">The BaseBehaviour holding on to this subscription.</param>
        public void ForceSubscribe(Action<T> action, BaseBehaviour context)
        {
            bool incidentExists = _incidentSubscriptions.ContainsKey(context);
            
            if (incidentExists)
            {
                _incidentSubscriptions[context] = action;
                return;
            }
            
            _incidentSubscriptions.Add(context, action);
        }

        public bool Unsubscribe(BaseBehaviour context)
        {
            return _incidentSubscriptions.Remove(context);
        }

        public void Publish(T incidentData)
        {
            foreach (var subscription in _incidentSubscriptions)
            {
                if (subscription.Key)
                {
                    subscription.Value?.Invoke(incidentData);
                }
            }
        }
    }
}