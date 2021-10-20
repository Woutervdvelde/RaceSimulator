using System;
using System.Text;
using System.Drawing;
using System.Windows.Media;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Windows.Media.Imaging;


namespace RaceSimulatorGUI
{
    public static class ResourceManager
    {
        private static Dictionary<string, Bitmap> _resources { get; set; }

        public static void Initialize()
        {
            _resources = new Dictionary<string, Bitmap>();
        }

        public static Bitmap GetImage(string input)
        {
            if (_resources.TryGetValue(input, out Bitmap img))
                return img;

            Bitmap image = new Bitmap(input);
            _resources.Add(input, image);
            return image;
        }

        public static void Clear()
        {
            _resources.Clear();
        }

        public static Bitmap GetEmptyImage(int w, int h)
        {
            if (_resources.TryGetValue("empty", out Bitmap img))
                return (Bitmap)img.Clone();

            Bitmap bitmap = new Bitmap(w, h);
            using (Graphics gfx = Graphics.FromImage(bitmap))
            using (SolidBrush brush = new SolidBrush(System.Drawing.Color.FromArgb(0, 0, 0)))
            {
                gfx.FillRectangle(brush, 0, 0, w, h);
            }

            _resources.Add("empty", bitmap);
            return (Bitmap)bitmap.Clone();
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
}
