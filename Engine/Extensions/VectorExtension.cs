using UnityEngine;

namespace UnityUtils.Engine.Extensions
{
    public static class VectorExtension
    {
        public static Vector2 Limited(this Vector2 v, float limitLength)
        {
            return v.normalized * limitLength;
        }

        public static Vector3 Limited(this Vector3 v, float limitLength)
        {
            return v.normalized * limitLength;
        }

        public static Vector4 Limited(this Vector4 v, float limitLength)
        {
            return v.normalized * limitLength;
        }

        public static void Limit(this Vector2 v, float limitLength)
        {
            v /= v.magnitude;
            v *= limitLength;
        }

        public static void Limit(this Vector3 v, float limitLength)
        {
            v /= v.magnitude;
            v *= limitLength;
        }

        public static void Limit(this Vector4 v, float limitLength)
        {
            v /= v.magnitude;
            v *= limitLength;
        }
    }
}