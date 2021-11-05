using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Controller;

namespace RaceSimulatorGUI
{
    class MainWindowDataContext : INotifyPropertyChanged
    {
        public string TrackName { get => Data.CurrentRace.Track.Name; }

        public MainWindowDataContext()
        {
            if (Data.CurrentRace != null)
                Data.CurrentRace.DriversChanged += OnDriversChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnDriversChanged(object sender, EventArgs args)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
