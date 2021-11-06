using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Controller;

namespace RaceSimulatorGUI
{
    public class MainWindowDataContext : INotifyPropertyChanged
    {
        public string TrackName { 
            get {
                return Data.CurrentRace.Track.Name;
            }}

        public MainWindowDataContext()
        {
            Race.RaceStarted += OnRaceStarted;
            Data.CurrentRace.DriversChanged += OnDriversChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnDriversChanged(object sender, EventArgs args)
        {
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public void OnRaceStarted(object sender, EventArgs args)
        {
            Data.CurrentRace.DriversChanged += OnDriversChanged;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TrackName"));
        }
    }
}
