using System;
using System.Collections.Generic;
using Firefly.Core.Behaviour;
using UnityEngine;

namespace Firefly.Core.Incident
{
    public abstract class BaseIncident<T>
    {
        private readonly Dictionary<BaseBehaviour, Action<T>> _incidentSubscriptions = new();

        protected abstract T IncidentInstance { get; } 
        
        /// <summary>
        /// A BaseBehaviour have only one subscription to an incident. This method throws an error if there is already a subscription on this context.
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="context"></param>
        public void BaseSubscribe(Action<T> subscription, BaseBehaviour context)
        {
            if (_incidentSubscriptions.ContainsKey(context))
            {
                Debug.LogError($"{context.name} is already subscribed to this incident!");
                return;
            }
            
            _incidentSubscriptions.Add(context, subscription);
        }
        
        /// <summary>
        /// A BaseBehaviour have only one subscription to an incident. This method returns false if there is already a subscription on this context.
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="context"></param>
        public bool TrySubscribe(Action<T> subscription, BaseBehaviour context)
        {
            if (_incidentSubscriptions.ContainsKey(context))
            {
                return false;
            }
            
            _incidentSubscriptions.Add(context, subscription);

            return true;
        }
        
        /// <summary>
        /// A BaseBehaviour have only one subscription to an incident. This method will overwrite the current subscription on this context if there is one.
        /// </summary>
        /// <param name="subscription"></param>
        /// <param name="context"></param>
        public void ForceSubscribe(Action<T> subscription, BaseBehaviour context)
        {
            if (_incidentSubscriptions.ContainsKey(context))
            {
                _incidentSubscriptions.Remove(context);
            }
            
            _incidentSubscriptions.Add(context, subscription);
        }

        public void Unsubscribe(BaseBehaviour context)
        {
            _incidentSubscriptions.Remove(context);
        }

        protected void Publish()
        {
            foreach (var subscription in _incidentSubscriptions.Values)
            {
                subscription?.Invoke(IncidentInstance);
            }
        }
    }
}