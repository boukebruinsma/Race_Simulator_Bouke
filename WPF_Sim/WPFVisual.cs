using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media.Imaging;

namespace WPF_Sim
{
    static class WPFVisual
    {
        public static BitmapSource DrawTrack(Track track)
        {
            return ImageProcessor.CreateBitmapSourceFromGdiBitmap(ImageProcessor.DrawBackground(1000, 750));

        }
    }
}
