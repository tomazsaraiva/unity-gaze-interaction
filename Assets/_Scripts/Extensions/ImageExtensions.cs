#region Includes
using UnityEngine;
using UnityEngine.UI;
#endregion

namespace TS.Extensions
{
    public static class ImageExtensions
    {
        #region Variables

        public enum ImageScaleMode
        {
            ScaleToFill,
            ScaleToFit
        }

        #endregion

        // IMPLEMENTED: scale to fill
        // TODO: implement stretch, scale to fit
        public static void SetSprite(this Image image, Sprite sprite, ImageScaleMode scaleMode = ImageScaleMode.ScaleToFill)
        {
            image.rectTransform.offsetMin = Vector2.zero;
            image.rectTransform.offsetMax = Vector2.zero;

            float aspectRatio = (float)sprite.texture.width / (float)sprite.texture.height;

            float offsetX = 0;
            float offsetY = 0;

            float targetHeight = image.rectTransform.rect.height;
            float targetWidth = image.rectTransform.rect.width;

            switch (scaleMode)
            {
                case ImageScaleMode.ScaleToFill:

                targetWidth = targetHeight * aspectRatio;

                if (targetWidth < image.rectTransform.rect.width)
                {
                    targetWidth = image.rectTransform.rect.width;
                    targetHeight = targetWidth / aspectRatio;
                }

                offsetX = Mathf.Abs(targetWidth - image.rectTransform.rect.width);
                offsetY = Mathf.Abs(targetHeight - image.rectTransform.rect.height);

                image.rectTransform.offsetMin = new Vector2(-offsetX, -offsetY);
                image.rectTransform.offsetMax = new Vector2(offsetX, offsetY);

                image.sprite = sprite;

                break;

                case ImageScaleMode.ScaleToFit:

                if (sprite.texture.width > sprite.texture.height)
                {
                    targetWidth = image.rectTransform.rect.width;
                }
                else
                {

                }

                break;
            }
        }
    }
}