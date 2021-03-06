using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Competition
    {
        public static event EventHandler CompetitionFinished;
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }
        public bool Done { get; set; }

        public Competition()
        {
            Participants = new List<IParticipant>();
            Tracks = new Queue<Track>();
        }
        public Track NextTrack()
        {
            if (Tracks.TryDequeue(out Track track))
                return track;
            Done = true;
            CompetitionFinished?.Invoke(this, new EventArgs());
            CompetitionFinished = null;
            return null;
        }
    }
}
