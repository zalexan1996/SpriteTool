
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Collections.Generic;

namespace ImagingLibrary
{
    /// <summary>
    /// A helper class for performing image manipulations.
    /// </summary>
    public sealed class ImageFactory
    {
        /// <summary>
        /// Takes a subsection of an image source
        /// </summary>
        /// <param name="SourceImage"> The image source to take a subsection of.</param>
        /// <param name="startingPoint">The starting (X,Y) coordinates of the subsection</param>
        /// <param name="width">The width of the new image.</param>
        /// <param name="height">The height of the new image</param>
        /// <returns>Returns a subsection of the provided image source.</returns>
        public static BitmapSource GetSubsection(BitmapSource SourceImage, Point startingPoint, int width, int height)
        {
            int stride = ((SourceImage.Format.BitsPerPixel + 7) / 8) * width;

            byte[] pixels = new byte[height * stride];
            SourceImage.CopyPixels(new Int32Rect((int)startingPoint.X, (int)startingPoint.Y, width, height), pixels, stride, 0);

            return BitmapSource.Create(width, height, SourceImage.DpiX, SourceImage.DpiY, SourceImage.Format, SourceImage.Palette, pixels, stride);
        }

        public static byte[] GetPixels(BitmapSource source, int offset = 0)
        {
            int stride = (int)(((source.Format.BitsPerPixel + 7) / 8) * source.PixelWidth);
            byte[] pixels = new byte[(int)source.PixelHeight * stride];
            source.CopyPixels(pixels, stride, offset);
            return pixels;
        }


        public static BitmapSource CombineImages(BitmapSource i1, BitmapSource i2)
        {
            if (i1 == null) { return null; }
            if (i2 == null) { return null; }

            byte[] pixels1 = GetPixels(i1);
            byte[] pixels2 = GetPixels(i2);

            int newPixelWidth = i1.PixelWidth + i2.PixelWidth;
            int newPixelHeight = i1.PixelHeight;

            byte[] newPixels = new byte[pixels1.Length + pixels2.Length];

            for (int y = 0; y < newPixelHeight; y++)
            {
                // Output the first row of i1 to newPixels
                for (int x = 0; x < i1.PixelWidth; x++)
                {
                    newPixels[y * newPixelWidth + x % newPixelWidth] = pixels1[y * i1.PixelHeight + x % i1.PixelWidth];
                }

                // Output the first row of i2 to newPixels
                for (int x = i1.PixelWidth; x < i1.PixelWidth + i2.PixelWidth; x++)
                {
                    newPixels[y * newPixelWidth + x % newPixelWidth] = pixels1[y * i2.PixelHeight + x % i2.PixelWidth];
                }
            }

            int newStride = (int)(((i1.Format.BitsPerPixel + 7) / 8) * newPixelWidth);
            return BitmapSource.Create(newPixelWidth, newPixelHeight, i1.DpiX, i1.DpiY, i1.Format, i1.Palette, newPixels, newStride);
        }

        public static void BitmapSourceToFile(BitmapSource source, string path)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(source));
            using (var filestream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                encoder.Save(filestream);
        }

        /// <summary>
        /// Converts a given screen position to a local-pixel position on the image. This cancels out any scaling.
        /// </summary>
        /// <param name="point">The point in screen space.</param>
        /// <param name="relativeTo">The image the point is relative to.</param>
        /// <returns>Returns the (X/Y) coordinate converted to local pixels.</returns>
        public static Point ConvertScreenToLocalPixels(Point point, Image relativeTo)
        {
            // Cache the image data.
            BitmapSource bmp = relativeTo.Source as BitmapSource;

            // Convert the screen coordinates to pixel coordinates in the image.
            return new Point(
                point.X * (bmp.PixelWidth / relativeTo.ActualWidth),
                point.Y * (bmp.PixelHeight / relativeTo.ActualHeight)
            );
        }

    }
}
