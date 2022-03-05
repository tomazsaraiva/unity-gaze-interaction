#region Includes
using UnityEngine;
#endregion

namespace WVF.Extensions
{
    public static class TransformExtensions
    {
        public static void SetPosition(this Transform transform, float? x, float? y, float? z, Space space = Space.World)
        {
            switch (space)
            {
                case Space.World:
                transform.position = UpdateVector(transform.position, x, y, z);
                break;
                case Space.Self:
                transform.localPosition = UpdateVector(transform.localPosition, x, y, z);
                break;
            }
        }
        public static void SetRotation(this Transform transform, float? x, float? y, float? z, Space space = Space.World)
        {
            switch (space)
            {
                case Space.World:
                transform.rotation = Quaternion.Euler(UpdateVector(transform.rotation.eulerAngles, x, y, z));
                break;
                case Space.Self:
                transform.localRotation = Quaternion.Euler(UpdateVector(transform.localRotation.eulerAngles, x, y, z));
                break;
            }
        }
        public static void SetScale(this Transform transform, float? x, float? y, float? z)
        {
            transform.localScale = UpdateVector(transform.localScale, x, y, z);
        }

        private static Vector3 UpdateVector(Vector3 vector, float? x, float? y, float? z)
        {
            if (x != null) { vector.x = x.Value; }
            if (y != null) { vector.y = y.Value; }
            if (z != null) { vector.z = z.Value; }
            return vector;
        }

        public static void Reset(this Transform transform, Space space)
        {
            switch (space)
            {
                case Space.World:
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
                break;
                case Space.Self:
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                break;
            }

            transform.localScale = Vector3.one;
        }
    }
}