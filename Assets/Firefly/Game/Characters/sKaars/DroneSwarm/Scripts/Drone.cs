using Firefly.Core;
using Unity.VisualScripting;
using UnityEngine;

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
        }

        protected override void OnDeactivate()
        {
            Swarm.LooseDrone(this);
            Swarm = null;
        }
    }
}