using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Model;
using Controller;
using System.Windows.Media.Imaging;
using System.Linq;

namespace RaceSimulatorGUI
{
    class RaceDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public List<RaceDriverPosition> RaceDriverPositions { get; set; }

        public RaceDataContext()
        {
            Race.DriversChanged += OnDriversChanged;
            GenerateDriverPositions();
        }

        public void GenerateDriverPositions()
        {
            var results = Data.CurrentRace.Participants
                .OrderByDescending(p => Data.CurrentRace.GetDistanceFromParticipant(p))
                .Select((p, i) => new RaceDriverPosition((Driver)p, i))
                .ToList();
            RaceDriverPositions = results;
        }

        public void OnDriversChanged(object sender, EventArgs args)
        {
            GenerateDriverPositions();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }

    public class RaceDriverPosition
    {
        public Driver Participant { get; set; }
        public int Position { get; set; }
        public string Name { get => Participant.Name; }
        public TimeSpan LapTime { 
            get 
            {
                if (Data.CurrentRace.LapTimes.TryGetValue(Participant, out LinkedList<TimeSpan> times))
                    return times.Last.Value;
                return TimeSpan.Zero;
            }
        }
        public BitmapSource Image { get => ResourceManager.CreateBitmapSourceFromGdiBitmap(ResourceManager.GetImage($".\\Resources\\{Participant.Image}")); }


        public RaceDriverPosition(Driver participant, int position)
        {
            Participant = participant;
            Position = position;
        }
    }
}
