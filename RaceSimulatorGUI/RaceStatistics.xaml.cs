using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Controller;

namespace RaceSimulatorGUI
{
    /// <summary>
    /// Interaction logic for RaceStatistics.xaml
    /// </summary>
    public partial class RaceStatistics : Window
    {
        public RaceStatistics()
        {
            InitializeComponent();
        }

        private void Force_NextRace(object sender, RoutedEventArgs e)
        {
            if (!Data.Competition.Done)
                Data.CurrentRace.Stop();
        }
    }
}
