using UnityEngine;

namespace Firefly.Core.Math
{
    public static class MonoMath
    {
        /// <summary>Returns the shortest angle in degrees from 0 to 180 between two floats.</summary>
        public static float Angle(float from, float to)
        {
            float angle = Mathf.Abs(to - from) % 360;
            return angle > 180 ? 360 - angle : angle;
        }

        /// <summary>Returns the shortest signed angle in degrees from 180 to -180 between two floats.</summary>
        public static float AngleSigned(float from, float to)
        {
            float angle = (to - from) % 360;
            if (angle > 180) return angle - 360;
            if (angle < -180) return angle + 360;
            return angle;
        }
		
        /// <summary>Remaps any angle to the range -180 to 180.</summary>
        public static float UnwarpAngle(float angle)
        {
            angle %= 360;
            if (angle > 180) return angle - 360;
            if (angle < -180) return angle + 360;
            return angle;
        }
    }
}