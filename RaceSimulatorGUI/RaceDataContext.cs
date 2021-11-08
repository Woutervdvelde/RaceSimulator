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
            LinkedList<IParticipant> participants = new LinkedList<IParticipant>();

            foreach (Section section in Data.CurrentRace.Track.Sections)
            {
                SectionData data = Data.CurrentRace.GetSectionData(section);
                if (data.Left != null && data.Right != null)
                {
                    if (data.DistanceLeft > data.DistanceRight)
                    {
                        participants.AddFirst(data.Left);
                        participants.AddFirst(data.Right);
                    }
                    else
                    {
                        participants.AddFirst(data.Right);
                        participants.AddFirst(data.Left);
                    }
                } 
                else if (data.Left != null)
                {
                    participants.AddFirst(data.Left);
                }
                else if (data.Right != null)
                {
                    participants.AddFirst(data.Right);
                }
            }

            //var results = Data.CurrentRace.Participants
            //    .OrderByDescending(p => Data.CurrentRace.GetDistanceFromParticipant(p))
            //    .Select((p, i) => new RaceDriverPosition((Driver)p, i))
            //    .ToList();
            //RaceDriverPositions = results;

            var results = participants
                .Select((p, i) => new RaceDriverPosition((Driver)p, i + 1))
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
