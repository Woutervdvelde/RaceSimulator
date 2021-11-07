using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Controller;
using Model;

namespace RaceSimulatorGUI
{
    public class MainWindowDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string TrackName { 
            get {
                return Data.CurrentRace.Track.Name;
            }}

        public MainWindowDataContext()
        {
            Race.RaceStarted += OnRaceStarted;
            Race.DriversChanged += OnDriversChanged;
        }

        public void OnDriversChanged(object sender, EventArgs args)
        {
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public void OnRaceStarted(object sender, EventArgs args)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TrackName"));
        }
    }
}
