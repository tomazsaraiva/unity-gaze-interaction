#region Includes
using UnityEngine;
#endregion

namespace TS.Extensions
{
    public static class ColorExtensions
    {
        public static Color ToBrightnessVariant(this Color color, float factor)
        {
            var red = color.r;
            var green = color.g;
            var blue = color.b;

            if (factor < 0)
            {
                factor = 1 + factor;
                red *= factor;
                green *= factor;
                blue *= factor;
            }
            else
            {
                red = (255 - red) * factor + red;
                green = (255 - green) * factor + green;
                blue = (255 - blue) * factor + blue;
            }

            return new Color(color.a, (int)red, (int)green, (int)blue);
        }
    }
}
