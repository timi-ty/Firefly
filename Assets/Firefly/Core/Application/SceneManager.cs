using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Firefly.Core.Application
{
    /// <summary>
    /// This is the entry point for every scene. Awake and start methods should never be used except here. Every member of a scene should come alive in OnAwaken().
    /// </summary>
    public class SceneManager : SingletonBehaviour<SceneManager>
    {
        private void Awake()
        {
            if (!Instance) AwakenScene();
        }

        private static void AwakenScene()
        {
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();

            List<GameObject> rootObjects = new();

            scene.GetRootGameObjects(rootObjects);

            int i = 0;
            foreach (GameObject rootObject in rootObjects)
            {
                IBaseBehaviour[] baseBehaviours = rootObject.GetComponents<IBaseBehaviour>();
                foreach (IBaseBehaviour baseBehaviour in baseBehaviours)
                {
                    if (baseBehaviour == null) continue;
                    
                    i++;
                    Debug.Log($"{nameof(SceneManager)}:::Waking up: {baseBehaviour.Name}");
                    baseBehaviour.Awaken();
                }
            }
            
            Debug.Log($"{nameof(SceneManager)}:::Scene Awoke | Root Obj Count: {rootObjects.Count} | Awoken Root Obj Count: {i}");
        }
    }
}