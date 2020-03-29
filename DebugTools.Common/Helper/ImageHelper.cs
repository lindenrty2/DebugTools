using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.IO;

namespace DebugTools.Common
{
    public class ImageHelper
    {
        public static Image GetImage(string path)
        {
            using (Stream stream = File.OpenRead(path))
            {
                return Bitmap.FromStream(stream);
            }
        }

        public static Image GetImage(string path, int x, int y, int width, int height)
        {
            return GetImage(path, new Rectangle(x, y, width, height));
        }

        public static Image GetImage(string path, Point point, Size size)
        {
            return GetImage(path, new Rectangle(point, size));
        }

        public static Image GetImage(string path, Rectangle rect)
        {
            try
            {
                Bitmap bitmap = (Bitmap)GetImage(path);
                Bitmap newImage = new Bitmap(rect.Size.Width, rect.Size.Height);
                newImage.SetResolution(72, 72);
                
                Graphics g = Graphics.FromImage(newImage);
                g.DrawImage(bitmap, 0, 0, rect, GraphicsUnit.Pixel);
                return newImage;
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
                return null;
            }
        }

        public static Image GetEmptyImage(Color color, int width,int height)
        {
            Bitmap newImage = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(newImage);
            g.Clear(color);
            return newImage;
        }

       

    }
}


