using System;
using System.Text;
using System.Drawing;
using System.Windows.Media;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Model;

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

            if (w == 0 && h == 0)
                return null;

            Bitmap bitmap = new Bitmap(w, h);
            using (Graphics gfx = Graphics.FromImage(bitmap))
            using (SolidBrush brush = new SolidBrush(System.Drawing.Color.FromArgb(255, 206, 101)))
            {
                gfx.FillRectangle(brush, 0, 0, w, h);
            }

            _resources.Add("empty", bitmap);
            return (Bitmap)bitmap.Clone();
        }

        public static Bitmap GetDriverImage(SectionSides side, TeamColors color, Section section)
        {
            /*
             * Participant resouces are made up of multiple images, 24 images for each color participant
             * The file name has been created to exists out of multiple parts devided by a "_"
             * 
             * For example: Blue_Corner_NE_East_Left.png
             * 1. TeamColor (Blue)
             * 2. SectionType (Corner)
             * 3. SectionDirection (NE)
             * 4. Participant direction (East)
             * 5. Side of the section (Left)
             * 
             * The SectionDirection (part 3 in the above example) only gets added when SectionType == Corner
             */

            StringBuilder url = new StringBuilder(".\\Resources\\");

            url.Append(Enum.GetName(typeof(TeamColors), color));
            url.Append("_");
            switch(section.SectionType)
            {
                case SectionTypes.StartGrid:
                case SectionTypes.Finish:
                case SectionTypes.Straight:
                    if (section.Direction == Directions.East || section.Direction == Directions.West)
                        url.Append("Horizontal_Straight");
                    else
                        url.Append("Vertical_Straight");
                    break;
                case SectionTypes.LeftCorner:
                case SectionTypes.RightCorner:
                    url.Append("Corner_");
                    url.Append(Enum.GetName(typeof(SectionDirections), section.SectionDirection));
                    break;
            }
            url.Append("_");
            url.Append(Enum.GetName(typeof(Directions), section.Direction));
            url.Append("_");
            url.Append(Enum.GetName(typeof(SectionSides), side));
            url.Append(".png");

            return GetImage(url.ToString());
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
