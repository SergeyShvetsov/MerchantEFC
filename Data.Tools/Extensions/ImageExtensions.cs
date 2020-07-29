using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;

namespace Data.Tools.Extensions
{
    public static class ImageExtensions
    {
        public static bool IsImage(this IFormFile file)
        {
            try
            {
                if (file == null || file.Length <= 0) return false;

                // Проверить расширение
                var ext = file.ContentType.ToLower();
                return (ext == "image/jpg"
                    || ext == "image/jpeg"
                    || ext == "image/pjpeg"
                    || ext == "image/gif"
                    || ext == "image/x-png"
                    || ext == "image/png");
            }
            catch
            {
                return false;
            }
        }
        // Based on SixLabors.ImageSharp
        public static Image GetImage(this IFormFile file)
        {
            if (!file.IsImage()) return null;
            return Image.Load(file.OpenReadStream());
        }
        public static Image Scale(this Image img, Size newSize)
        {
            var scaled = img.ScaledImageSize(newSize);
            img.Mutate(x => x.Resize(scaled));
            return img;
        }
        public static Image Scale(this Image img, int width, int height)
        {
            return img.Scale(new Size(width, height));
        }

        public static Size ScaledImageSize(this Image img, Size newSize)
        {

            var ratioX = (double)newSize.Width / img.Width;
            var ratioY = (double)newSize.Height / img.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(img.Width * ratio);
            var newHeight = (int)(img.Height * ratio);

            return new Size(newWidth, newHeight);
        }
    }
}
