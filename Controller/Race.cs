using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Model;

namespace Controller
{
    public class Race
    {
        /**
         * Important rules for position players:
         *  - Maximum of 2 participants on a section (left and right)
         *  - Keep track of the position of a participant on a section
         *  
         *  Own rules:
         *  - Track must start with all the StartGrids (min of 3)
         *  - The last Section should always be the Finish
         */
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public int Laps { get; set; }

        public DateTime StartTime { get; set; }
        public event EventHandler DriversChanged;
        public event EventHandler RaceFinished;

        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private Dictionary<IParticipant, int> _laps;
        private LinkedList<IParticipant> _leaderboard;
        private Timer _timer;
        private bool _needsUpdate;

        public Race(Track track, List<IParticipant> participants, int laps)
        {
            Track = track;
            Participants = participants;
            Laps = laps;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            InitializeLaps();
            _leaderboard = new LinkedList<IParticipant>();
            _timer = new Timer(500);
            _timer.Elapsed += OnTimedEvent;
            PositionParticipants(track, participants);
            RandomizeEquipment();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            RaceFinished?.Invoke(this, new EventArgs());
            _timer = null;
            DriversChanged = null;
            RaceFinished = null;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs args)
        {
            MoveParticipants();
        }

        public SectionData GetSectionData(Section section)
        {
            if (_positions.TryGetValue(section, out SectionData sectionData))
                return sectionData;

            sectionData = new SectionData();
            sectionData.Section = section;
            _positions.Add(section, sectionData);
            return sectionData;
        }

        private void RandomizeEquipment()
        {
            Participants.ForEach(_participant =>
            {
                _participant.Equipment.Quality = _random.Next(3, 10);
                _participant.Equipment.Performance = _random.Next(3, 10);
                _participant.Equipment.Speed = _random.Next(3, 10);
            });
        }

        public void PositionParticipants(Track track, List<IParticipant> participants)
        {
            LinkedList<Section> startSections = track.GetStartSections();

            if (startSections.Count * 2 < participants.Count)
                throw new Exception("Not enough start grids for participants");

            for (int i = 0; i < participants.Count; i++)
            {
                int index = Math.Abs(i / 2);
                SectionData data = GetSectionData(startSections.ElementAt(index));

                if (i % 2 == 0)
                {
                    data.Left = participants[i];
                    participants[i].IsLeft = true;
                }
                else
                {
                    data.Right = participants[i];
                    participants[i].IsLeft = false;
                }
            }
        }

        public void InitializeLaps()
        {
            _laps = new Dictionary<IParticipant, int>();

            foreach (IParticipant p in Participants)
                _laps.Add(p, 0);
        }

        public void AddParticipantLap(IParticipant p)
        {
            _laps[p] += 1;
            if (_laps[p] >= Laps)
                ParticipantFinished(p);
        }

        public void ParticipantFinished(IParticipant p)
        {
            SectionData data = GetSectionData(Track.Sections.First.Value);
            if (data.Left == p)
                data.Left = null;
            if (data.Right == p)
                data.Right = null;

            _leaderboard.AddLast(p);
            CheckFinished();
        }

        public void CheckFinished()
        {
            if (_leaderboard.Count == Participants.Count)
                Stop();
        }

        public void MoveParticipants()
        {
            foreach (Section section in Track.Sections)
            {
                SectionData data = GetSectionData(section);
                if (data.Left != null)
                    data.DistanceLeft += data.Left.Equipment.Performance * data.Left.Equipment.Speed;

                if (data.Right != null)
                    data.DistanceRight += data.Right.Equipment.Performance * data.Right.Equipment.Speed;

                if (data.DistanceLeft >= Section.Length)
                    MoveToNext(section, data.Left);

                if (data.DistanceRight >= Section.Length)
                    MoveToNext(section, data.Right);
            }

            CheckAllWaiting();

            if (_needsUpdate)
            {
                DriversChangedEventArgs args = new DriversChangedEventArgs(Track);
                DriversChanged?.Invoke(this, args);
                _needsUpdate = false;
            }
        }

        public void MoveToNext(Section currentSection, IParticipant participant)
        {
            Section nextSection = Track.GetNextSection(currentSection);
            SectionData nextData = GetSectionData(nextSection);

            if (!nextData.Waiting.Contains(participant))
                nextData.Waiting.Enqueue(participant);
        }

        public void CheckAllWaiting()
        {
            var section = Track.Sections.Last;
            while (section != null)
            {
                SectionData data = GetSectionData(section.Value);
                if (data.Waiting.Count > 0)
                    CheckWaiting(section.Value, data);
                section = section.Previous;
            }
        }

        public void CheckWaiting(Section currentSection, SectionData data)
        {
            Section prevSection = Track.GetPreviousSection(currentSection);
            SectionData prevData = GetSectionData(prevSection);

            if (data.Left != null && data.Right != null)
                return;

            IParticipant p = data.Waiting.Dequeue();
            MoveParticipantData(data, prevData, p);

            if (data.Waiting.Count > 0 && (data.Left == null || data.Right == null))
            {
                IParticipant p2 = data.Waiting.Dequeue();
                MoveParticipantData(data, prevData, p2);
            }

            _needsUpdate = true;
        }

        public void MoveParticipantData(SectionData data, SectionData prevData, IParticipant p)
        {
            if (data.Left == null)
            {
                data.Left = p;
                if (p.IsLeft)
                {
                    prevData.Left = null;
                    prevData.DistanceLeft = 0;
                }
                else
                {
                    prevData.Right = null;
                    prevData.DistanceRight = 0;
                }
                p.IsLeft = true;
            }
            else if (data.Right == null)
            {
                data.Right = p;
                if (p.IsLeft)
                {
                    prevData.Left = null;
                    prevData.DistanceLeft = 0;
                }
                else
                {
                    prevData.Right = null;
                    prevData.DistanceRight = 0;
                }
                p.IsLeft = false;
            }

            if (prevData.Section.SectionType == SectionTypes.Finish)
                AddParticipantLap(p);
        }
    }
}
