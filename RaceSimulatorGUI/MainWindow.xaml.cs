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

            ResourceManager.Initialize();
            Visual.Initialize(Data.CurrentRace);
            Visual.DrawTrack(Data.CurrentRace.Track);

            Data.CurrentRace.RaceFinished += NextRace;
            Data.CurrentRace.Start();

            Data.CurrentRace.DriversChanged += OnDriversChanged;
        }

        void NextRace(object source, EventArgs args)
        {
            Data.NextRace();
            if (Data.Competition.Done)
                return;

            Visual.Initialize(Data.CurrentRace);
            Visual.DrawTrack(Data.CurrentRace.Track);

            Data.CurrentRace.RaceFinished += NextRace;
            Data.CurrentRace.Start();
        }

        void OnDriversChanged(object source, EventArgs args)
        {
            DriversChangedEventArgs driverArgs = args as DriversChangedEventArgs;
            this.Track.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(() => {
                this.Track.Source = null;
                this.Track.Source = Visual.DrawTrack(driverArgs.Track);
            }));
        }
    }
}
