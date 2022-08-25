using System;
using Firefly.Game.Management;
using UnityEngine;

namespace Firefly.Core.Tests
{
    public class IncidentBenchmark : BaseBehaviour
    {
        private void Start()
        {
            //subscribers
            int subscriberCount = 2000;
            for (int i = 0; i < subscriberCount; i++)
            {
                BaseBehaviour bB = new GameObject().AddComponent<TestBehaviour>();
                TestIncident.Instance.Subscribe(data => { int i = 0; }, bB);
            }
            
            int publishCount = 100000;
            var incidentData = new TestIncident.Data();
            long startTime = DateTime.Now.Ticks;
            for (int i = 0; i < publishCount; i++)
            {
                TestIncident.Instance.Publish(ref incidentData);
            }
            long endTime = DateTime.Now.Ticks;

            float milliSecs = (endTime - startTime) / 10000.0f;
            
            Debug.Log($"{publishCount} Incidents took {milliSecs}ms to publish to {subscriberCount} subscribers each.");
        }
    }
}