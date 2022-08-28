using Firefly.Core;
using UnityEngine;

namespace Firefly.Game
{
    public class Firefly : SingletonBehaviour<Firefly>
    {
        public static Vector3 Position => Instance.transform.position;
    }
}