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

        public string TrackName => Data.CurrentRace.Track.Name;

        public MainWindowDataContext()
        {
            Race.RaceStarted += OnRaceStarted;
        }

        public void OnRaceStarted(object sender, EventArgs args)
        {
            string name = Data.CurrentRace.Track.Name;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}
