using System;
using Firefly.Core;
using Firefly.Core.Math;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Firefly.Game
{
    public class Drone : PoolableGameObject, IHealth
    {
        #region Component References

        private Rigidbody RigidBody { get; set; }

        #endregion
        
        #region Properties

        private DroneSwarm Swarm { get; set; }
        public Health Health { private set; get; }
        private Vector3 PositionInSwarm { get; set; }
        private float CohesiveStrength { get; set; }
        private float RestFactor { get; set; }

        #endregion

        #region Worker Fields

        private Vector3 _lastExtension;
        private Vector3 _deltaExtension;
        private int _avoidLeftOrRight;

        #endregion


        protected override void OnAwaken()
        {
            Health = new Health(100);
            IgnoreLifetime = true;
            RigidBody = GetComponent<Rigidbody>();
        }

        protected void FixedUpdate()
        {
            FollowSwarm();
            AvoidFirefly();
        }

        private void FollowSwarm()
        {
            Vector3 extension = RigidBody.position - (Swarm.transform.position + PositionInSwarm);
            _deltaExtension = extension - _lastExtension;
            Vector3 dampener = RestFactor * (_deltaExtension / Time.fixedDeltaTime);

            Vector3 cohesiveForce = -(CohesiveStrength * extension) - dampener;
            RigidBody.AddForce(cohesiveForce);

            _lastExtension = extension;
        }

        private void AvoidFirefly()
        {
            Vector3 position = RigidBody.position;
            Vector3 fireflyDisplacement = (position - Firefly.Position).X0Z();
            float avoidanceStrength = 2 / Mathf.Max(fireflyDisplacement.sqrMagnitude, 0.001f);
            Vector3 avoidanceDirection = Vector3.Cross(fireflyDisplacement, Vector3.up) * _avoidLeftOrRight;
            avoidanceDirection.Normalize();
            Vector3 avoidanceForce = avoidanceStrength * avoidanceDirection;
            if (fireflyDisplacement.sqrMagnitude < 49) RigidBody.AddForce(avoidanceForce, ForceMode.VelocityChange);
        }

        public void JoinSwarm(DroneSwarm swarm, Vector3 localPosition)
        {
            Swarm = swarm;
            PositionInSwarm = localPosition;
            transform.position = localPosition;
            CohesiveStrength = Random.Range(0.5f, 4f);
            RestFactor = Random.Range(0.5f, 4f);
        }

        protected override void OnActivate()
        {
            Health.Refill();
            _avoidLeftOrRight = Random.Range(0f, 1f) > 0.5f ? -1 : 1;
        }

        protected override void OnDeactivate()
        {
            Swarm.LooseDrone(this);
            Swarm = null;
        }
    }
}