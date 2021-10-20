using System;
using System.Text;
using Model;
using Controller;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace RaceSimulatorGUI
{
    public static class Visual
    {
        public static void Initialize(Race race)
        {

        }

        public static BitmapSource DrawTrack(Track track)
        {
            BitmapSource b = ResourceManager.CreateBitmapSourceFromGdiBitmap(ResourceManager.GetEmptyImage(250, 250));
            return b;
        }
    }
}
