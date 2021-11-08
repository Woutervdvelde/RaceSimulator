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
        public List<LapTimeSpan> LapTimeSpans { get; set; }

        public RaceDataContext()
        {
            Race.DriversChanged += OnDriversChanged;
            GenerateDriverPositions();
            GenerateLapTimes();
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

            RaceDriverPositions = participants
                .Select((p, i) => new RaceDriverPosition((Driver)p, i + 1))
                .OrderByDescending(p => p.Laps)
                .ToList();
        }

        public void GenerateLapTimes()
        {
            List<LapTimeSpan> list = new List<LapTimeSpan>();
            foreach (KeyValuePair<IParticipant, LinkedList<TimeSpan>> entry in Data.CurrentRace.LapTimes)
                foreach (TimeSpan time in entry.Value)
                    list.Add(new LapTimeSpan((Driver)entry.Key, time));

            LapTimeSpans = list.OrderBy(l => l.LapTime).ToList();
        }

        public void OnDriversChanged(object sender, EventArgs args)
        {
            GenerateDriverPositions();
            GenerateLapTimes();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }

    public class RaceDriverPosition
    {
        public Driver Participant { get; set; }

        //TODO: position doesn't show correctly
        public int Position { get; set; }
        public int Laps { get => Data.CurrentRace.ParticipantLaps[Participant]; }
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

    public class LapTimeSpan {
        public Driver Participant { get; set; }
        public TimeSpan LapTime { get; set; }
        public BitmapSource Image { get => ResourceManager.CreateBitmapSourceFromGdiBitmap(ResourceManager.GetImage($".\\Resources\\{Participant.Image}")); }


        public LapTimeSpan(Driver participant, TimeSpan timeSpan)
        {
            Participant = participant;
            LapTime = timeSpan;
        }
    }
}
