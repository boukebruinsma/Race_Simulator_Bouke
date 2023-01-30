using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public static class ImageProcessor
{
    private static Dictionary<string, Bitmap> _cache = new Dictionary<string, Bitmap>();

    public static Bitmap InsertImage(string url)
    {
        if (_cache.TryGetValue(url, out var data))
        {
            return data;
        }

        Bitmap bmp = new Bitmap(url);
        _cache.Add(url, bmp);
        return bmp;
    }

    public static Bitmap DrawSectorImage(string url, int x, int y, int direction, int distanceBetweenDrivers)
    {
        Bitmap bg = _cache["empty"];

        Bitmap sector = InsertImage(url);
        sector = new Bitmap(sector, new Size(sector.Width / 5, sector.Height / 5));

        for (int i = 0; i < direction; i++)
        {
            sector.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        if(direction == 0)
        {
            x += distanceBetweenDrivers;
        }
        else if(direction == 1)
        {
            y += distanceBetweenDrivers;
        }
        else if(direction == 2)
        {
            x -= distanceBetweenDrivers; 
        }
        else if(direction == 3)
        {
            y -= distanceBetweenDrivers;
        }

        Graphics g = Graphics.FromImage(bg);
        g.DrawImage(sector, new Point(x, y));
        return bg;
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
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(0, 255, 82, 32)), ImageSize);
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
