using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Firefly.Core.Application
{
    /// <summary>
    /// This is the entry point for every scene. Awake and start methods should never be used except here. Every member of a scene should come alive in OnAwaken().
    /// </summary>
    public class SceneManager : BaseBehaviour
    {
        private static SceneManager Instance { get; set; }
        
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                AwakenScene();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private static void AwakenScene()
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            List<GameObject> rootObjects = new List<GameObject>();

            scene.GetRootGameObjects(rootObjects);

            int i = 0;
            foreach (var rootObject in rootObjects)
            {
                BaseBehaviour baseBehaviour = rootObject.GetComponent<BaseBehaviour>();
                if (baseBehaviour)
                {
                    i++;
                    Debug.Log($"{nameof(SceneManager)}:::Waking up: {baseBehaviour.name}");
                    baseBehaviour.Awaken();
                }
            }
            
            Debug.Log($"{nameof(SceneManager)}:::Scene Awoke | Root Obj Count: {rootObjects.Count} | Awoken Root Obj Count: {i}");
        }
    }
}