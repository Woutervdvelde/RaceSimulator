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
                return $"Track: {Data.CurrentRace.Track.Name}";
            }}

        public MainWindowDataContext()
        {
            Data.CurrentRace.DriversChanged += OnDriversChanged;
            OnDriversChanged(this, new EventArgs());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnDriversChanged(object sender, EventArgs args)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
