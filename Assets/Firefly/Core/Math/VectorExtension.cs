using UnityEngine;

namespace Firefly.Core.Math
{
    public static class VectorExtension
    {
        public static Vector3 X0Z(this Vector3 vector3)
        {
            return new Vector3(vector3.x, 0, vector3.z);
        }
    }
}