#region Includes
using UnityEngine;
#endregion

namespace WVF.Extensions
{
    public static class RendererExtensions
    {
        public static Rect GetScreenRect(this Renderer renderer)
        {
            var center = renderer.bounds.center;
            var extents = renderer.bounds.extents;
            var camera = Camera.main;

            Vector2 min = camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z));
            Vector2 max = min;

            //0
            Vector2 point = min;
            GetMinMax(point, ref min, ref max);

            //1
            point = camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z));
            GetMinMax(point, ref min, ref max);

            //2
            point = camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z));
            GetMinMax(point, ref min, ref max);

            //3
            point = camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z));
            GetMinMax(point, ref min, ref max);

            //4
            point = camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z));
            GetMinMax(point, ref min, ref max);

            //5
            point = camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z));
            GetMinMax(point, ref min, ref max);

            //6
            point = camera.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z));
            GetMinMax(point, ref min, ref max);

            //7
            point = camera.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z));
            GetMinMax(point, ref min, ref max);

            return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        }

        static void GetMinMax(Vector2 point, ref Vector2 min, ref Vector2 max)
        {
            min = new Vector2(min.x >= point.x ? point.x : min.x, min.y >= point.y ? point.y : min.y);
            max = new Vector2(max.x <= point.x ? point.x : max.x, max.y <= point.y ? point.y : max.y);
        }
    }
}