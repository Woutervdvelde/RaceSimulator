using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Linq;
using Model;
using Controller;
using System.Windows.Media.Imaging;

namespace RaceSimulatorGUI
{
    class CompetitionDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public List<CompetitionRanking> CompetitionRankings { get; set; }
        public string CurrentRaceName { get => Data.CurrentRace.Track.Name; }
        public string NextRaceName { 
            get {
                Data.Competition.Tracks.TryPeek(out Track t);
                if (t != null)
                    return t.Name;
                return ""; 
            }
        }

        public CompetitionDataContext()
        {
            RankParticipants(Data.Competition.Participants);
            Race.RaceFinished += OnRaceFinished;
        }

        public void RankParticipants(List<IParticipant> participants)
        {
            participants.Sort(SortByPoints);
            CompetitionRankings = participants.Select((p, i) => new CompetitionRanking((Driver)p, i + 1)).ToList();
        }

        public int SortByPoints(IParticipant a, IParticipant b)
        {
            if (a.Points == null && b.Points == null) return 0;
            else if (a.Points == null) return 1;
            else if (b.Points == null) return -1;
            else return b.Points.CompareTo(a.Points);
        }

        public void OnRaceFinished(object sender, EventArgs args)
        {
            RankParticipants(Data.Competition.Participants);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }

    public class CompetitionRanking
    {
        public Driver Participant { get; set; }
        public int Rank { get; set; }
        public string Name { get => Participant.Name; }
        public int Points { get => Participant.Points; }
        public BitmapSource Image { get => ResourceManager.CreateBitmapSourceFromGdiBitmap(ResourceManager.GetImage($".\\Resources\\{Participant.Image}")); } 

        public CompetitionRanking(Driver participant, int rank)
        {
            Participant = participant;
            Rank = rank;
        }
    }
}
