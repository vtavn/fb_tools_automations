﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Facebook_Tool_Request.Helpers
{
    internal class ImageScanOpenCV
    {
        public static Bitmap GetImage(string path)
        {
            return new Bitmap(path);
        }

        public static Bitmap Find(string main, string sub, double percent = 0.9)
        {
            Bitmap image = ImageScanOpenCV.GetImage(main);
            Bitmap image2 = ImageScanOpenCV.GetImage(sub);
            return ImageScanOpenCV.Find(main, sub, percent);
        }

        public static Bitmap Find(Bitmap mainBitmap, Bitmap subBitmap, double percent = 0.9)
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(mainBitmap);
            Image<Bgr, byte> image2 = new Image<Bgr, byte>(subBitmap);
            Image<Bgr, byte> image3 = image.Copy();
            using (Image<Gray, float> image4 = image.MatchTemplate(image2, TemplateMatchingType.CcoeffNormed))
            {
                double[] array;
                double[] array2;
                Point[] array3;
                Point[] array4;
                image4.MinMax(out array, out array2, out array3, out array4);
                bool flag = array2[0] > percent;
                bool flag2 = flag;
                if (flag2)
                {
                    Rectangle rect = new Rectangle(array4[0], image2.Size);
                    image3.Draw(rect, new Bgr(Color.Red), 2, LineType.EightConnected, 0);
                }
                else
                {
                    image3 = null;
                }
            }
            return (image3 == null) ? null : image3.ToBitmap();
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static Point? FindOutPoint(Bitmap mainBitmap, Bitmap subBitmap, double percent = 0.9)
        {
            bool flag = subBitmap == null || mainBitmap == null;
            bool flag2 = flag;
            Point? result;
            if (flag2)
            {
                result = null;
            }
            else
            {
                bool flag3 = subBitmap.Width > mainBitmap.Width || subBitmap.Height > mainBitmap.Height;
                bool flag4 = flag3;
                if (flag4)
                {
                    result = null;
                }
                else
                {
                    Image<Bgr, byte> image = new Image<Bgr, byte>(mainBitmap);
                    Image<Bgr, byte> template = new Image<Bgr, byte>(subBitmap);
                    Point? point = null;
                    using (Image<Gray, float> image2 = image.MatchTemplate(template, TemplateMatchingType.CcoeffNormed))
                    {
                        double[] array;
                        double[] array2;
                        Point[] array3;
                        Point[] array4;
                        image2.MinMax(out array, out array2, out array3, out array4);
                        bool flag5 = array2[0] > percent;
                        bool flag6 = flag5;
                        if (flag6)
                        {
                            point = new Point?(array4[0]);
                        }
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    result = point;
                }
            }
            return result;
        }
        public static List<Point> FindOutPoints(Bitmap mainBitmap, Bitmap subBitmap, double percent = 0.9)
        {
            Image<Bgr, byte> image = new Image<Bgr, byte>(mainBitmap);
            Image<Bgr, byte> image2 = new Image<Bgr, byte>(subBitmap);
            List<Point> list = new List<Point>();
            for (; ; )
            {
                using (Image<Gray, float> image3 = image.MatchTemplate(image2, TemplateMatchingType.CcoeffNormed))
                {
                    double[] array;
                    double[] array2;
                    Point[] array3;
                    Point[] array4;
                    image3.MinMax(out array, out array2, out array3, out array4);
                    bool flag = array2[0] > percent;
                    bool flag2 = !flag;
                    if (flag2)
                    {
                        break;
                    }
                    Rectangle rect = new Rectangle(array4[0], image2.Size);
                    image.Draw(rect, new Bgr(Color.Blue), -1, LineType.EightConnected, 0);
                    list.Add(array4[0]);
                }
            }
            return list;
        }
        public static List<Point> FindColor(Bitmap mainBitmap, Color color)
        {
            int num = color.ToArgb();
            List<Point> list = new List<Point>();
            try
            {
                for (int i = 0; i < mainBitmap.Width; i++)
                {
                    for (int j = 0; j < mainBitmap.Height; j++)
                    {
                        bool flag = num.Equals(mainBitmap.GetPixel(i, j).ToArgb());
                        bool flag2 = flag;
                        if (flag2)
                        {
                            list.Add(new Point(i, j));
                        }
                    }
                }
            }
            finally
            {
                bool flag3 = mainBitmap != null;
                if (flag3)
                {
                    ((IDisposable)mainBitmap).Dispose();
                }
            }
            return list;
        }
    }

}
