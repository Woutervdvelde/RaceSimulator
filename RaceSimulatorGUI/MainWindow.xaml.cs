using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Model;
using Controller;

namespace RaceSimulatorGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Data.Initialize();
            Data.NextRace();

            Visual.Initialize(Data.CurrentRace);
            DrawTrack(Data.CurrentRace.Track);

            Data.CurrentRace.RaceFinished += NextRace;
            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Data.CurrentRace.Start();
        }

        void NextRace(object source, EventArgs args)
        {
            Data.NextRace();
            if (Data.Competition.Done)
                return;

            Visual.Initialize(Data.CurrentRace);
            DrawTrack(Data.CurrentRace.Track);
            Data.CurrentRace.RaceFinished += NextRace;
            Data.CurrentRace.DriversChanged += OnDriversChanged;
            Data.CurrentRace.Start();

        }

        void OnDriversChanged(object source, EventArgs args)
        {
            DriversChangedEventArgs driverArgs = args as DriversChangedEventArgs;
            DrawTrack(driverArgs.Track);
        }

        void DrawTrack(Track track)
        {
            this.Track.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() => {
                this.Track.Source = null;
                this.Track.Source = Visual.DrawTrack(track);
            }));
        }

        private void MenuItem_MenuItem_Race_Click(object sender, RoutedEventArgs args)
        {
            RaceStatistics stats = new RaceStatistics();
            stats.Show();
        }

        private void MenuItem_MenuItem_Competition_Click(object sender, RoutedEventArgs args)
        {
            CompetitionStatistics stats = new CompetitionStatistics();
            stats.Show();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }
    }
}
