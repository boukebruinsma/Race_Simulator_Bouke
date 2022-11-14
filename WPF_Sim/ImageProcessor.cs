using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public static class ImageProcessor
{
    private static Dictionary<string, Bitmap> _cache = new Dictionary<string, Bitmap>();

    public static Bitmap BitmapFunction(string url)
    {
        if (_cache.TryGetValue(url, out var data))
        {
            return data;
        }

        Bitmap bmp = new Bitmap(url);
        _cache.Add(url, bmp);
        return bmp;
    }

    public static void ClearCache()
    {
        _cache.Clear();
    }

    public static Bitmap DrawBackground(int width, int height)
    {
        if (_cache.TryGetValue("empty", out var data))
        {
            return (Bitmap)data.Clone();
        }
        Bitmap bmp = new Bitmap(width, height);
        using (Graphics graph = Graphics.FromImage(bmp))
        {
            Rectangle ImageSize = new Rectangle(0, 0, width, height);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.Green), ImageSize);
        }

        _cache.Add("empty", bmp);
        

        return (Bitmap)bmp.Clone();
    }

    public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap)
    {
        if (bitmap == null)
            throw new ArgumentNullException("bitmap");
        var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        var bitmapData = bitmap.LockBits(
        rect,
        ImageLockMode.ReadWrite,
        System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        try
        {
            var size = (rect.Width * rect.Height) * 4;
            return BitmapSource.Create(
            bitmap.Width,
            bitmap.Height,
            bitmap.HorizontalResolution,
            bitmap.VerticalResolution,
            PixelFormats.Bgra32,
            null,
            bitmapData.Scan0,
            size,
            bitmapData.Stride);
        }
        finally
        {
            bitmap.UnlockBits(bitmapData);
        }
    }
}
