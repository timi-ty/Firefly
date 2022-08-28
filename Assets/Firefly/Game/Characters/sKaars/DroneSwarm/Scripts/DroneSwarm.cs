using System;
using System.Collections.Generic;
using Firefly.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Firefly.Game
{
    public class DroneSwarm : BaseBehaviour
    {
        #region Swarm Settings

        [Header("Swarm Settings")][SerializeField] 
        private int _swarmSize;
        [SerializeField] 
        private float _swarmRadius;

        #endregion

        [SerializeReference] 
        private DronePoolHolder _dronePoolHolder;

        private HashSet<Drone> _activeDrones;

        protected override void OnAwaken()
        {
            _activeDrones = new HashSet<Drone>();
            
            for (int i = 0; i < _swarmSize; i++)
            {
                Drone drone = _dronePoolHolder.Acquire();
                _activeDrones.Add(drone);
                drone.JoinSwarm(this, Random.insideUnitSphere * _swarmRadius);
            }
        }

        public void LooseDrone(Drone drone)
        {
            if (_activeDrones.Remove(drone)) return;
            
            Debug.LogWarning($"Tried to loose {drone}, but it does not belong to {this}");
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, _swarmRadius);
        }
    }
}