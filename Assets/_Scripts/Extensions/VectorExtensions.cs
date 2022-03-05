#region Includes
using UnityEngine;
#endregion

namespace AttentionLab.Extensions
{
    public static class Vector2Extensions
    {
        public static float AngleTo(this Vector2 first, Vector2 second)
        {
            return Mathf.Atan2(second.y - first.y, second.x - first.x) * 180 / Mathf.PI;
        }
        public static Vector3 PointAtDistance(this Vector3 first, Vector3 second, float distance)
        {
            return distance * Vector3.Normalize(second - first) + first;
        }
    }
}
