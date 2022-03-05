#region Includes
using System;
using UnityEngine;
#endregion

namespace TS.Extensions
{
    public static class BoundsExtensions
    {
        /// <summary>
        /// Returns the largest side (X,Y,Z) value of the given Bounds.
        /// </summary>
        /// <param name="bounds">A not null <c>Bounds</c> value</param>
        /// <returns>Returns the value of the largest side</returns>
        /// <exception cref="ArgumentNullException">Thrown when the given bounds is null</exception>
        public static float MaxDimension(this Bounds bounds)
        {
            if (bounds == null)
            {
                throw new ArgumentNullException("bounds");
            }

            return Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z) / 2;
        }
    }
}