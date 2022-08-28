using System;
using System.Collections.Generic;
using Firefly.Core;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Firefly.Game
{
    public class DroneSwarm : BaseBehaviour
    {
        #region Serilized Fields and References

        [Header("Swarm Settings")][SerializeField] 
        private int _swarmSize;
        
        [SerializeField] 
        private float _swarmRadius;
        
        [Header("Drone Pool")][SerializeReference] 
        private DronePoolHolder _dronePoolHolder;

        [Header("Movement Settings")] [SerializeField]
        private Vector2 _randomRangeLimits = new(2, 5);

        [SerializeField] 
        private float _dispersionStrength = 1f;

        #endregion

        #region Component References

        private NavMeshAgent NavAgent { get; set; }

        #endregion

        #region Worker Fields

        private readonly HashSet<Drone> _activeDrones = new();

        #endregion

        protected override void OnAwaken()
        {
            NavAgent = GetComponent<NavMeshAgent>();
            NavAgent.SetDestination(transform.position);

            AcquireDrones();
        }

        private void Update()
        {
            if (NavAgent.remainingDistance <= NavAgent.stoppingDistance)
            {
                SetNewDestination();
            }
        }

        private void SetNewDestination()
        {
            Vector3 origin = Firefly.Position;
            float maxRange = Random.Range(_randomRangeLimits.x, _randomRangeLimits.y);
			
            bool foundPosition = NavMesh.SamplePosition(
                origin + Random.insideUnitSphere * maxRange,
                out NavMeshHit navMeshHit,
                maxRange,
                NavMesh.AllAreas
            );

            if (!foundPosition) return;
            
            NavAgent.SetDestination(navMeshHit.position);
        }

        private void AcquireDrones()
        {
            for (int i = 0; i < _swarmSize; i++)
            {
                Drone drone = _dronePoolHolder.Acquire();
                _activeDrones.Add(drone);
                drone.JoinSwarm(this, Random.insideUnitSphere * _swarmRadius);
            }
        }

        public void LooseDrone(Drone drone)
        {
            if (_activeDrones.Remove(drone))
            {
                if (_activeDrones.Count == 0) Die();
                
                return;
            }

            Debug.LogWarning($"Tried to loose {drone}, but it does not belong to {this}");
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position, _swarmRadius);
        }

        private void Die()
        {
            
        }
    }
}