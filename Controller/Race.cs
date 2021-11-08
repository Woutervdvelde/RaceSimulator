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
        // TODO: make delegate for DriversChanged
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public int Laps { get; set; }
        public Dictionary<IParticipant, int> ParticipantLaps;
        public Dictionary<IParticipant, LinkedList<TimeSpan>> LapTimes;

        public static event EventHandler DriversChanged;
        public static event EventHandler RaceFinished;
        public static event EventHandler RaceStarted;

        public LinkedList<IParticipant> Leaderboard;

        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private Timer _timer;
        private DateTime _startTime;
        private bool _needsUpdate;

        public Race(Track track, List<IParticipant> participants, int laps)
        {
            Track = track;
            Participants = participants;
            Leaderboard = new LinkedList<IParticipant>();
            Laps = laps;
            LapTimes = new Dictionary<IParticipant, LinkedList<TimeSpan>>();

            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            _timer = new Timer(500);
            _timer.Elapsed += OnTimedEvent;

            InitializeLaps();
            PositionParticipants(track, participants);
            RandomizeEquipment();

            Competition.CompetitionFinished += OnCompetitionFinished;
            RaceStarted?.Invoke(this, new EventArgs());
        }

        public void Start()
        {
            _timer.Start();
            _startTime = DateTime.Now;
        }

        public void Stop()
        {
            RaceFinished?.Invoke(this, new EventArgs());
            _timer = null;
            Competition.CompetitionFinished -= OnCompetitionFinished;
        }

        public void OnCompetitionFinished(object sender, EventArgs args)
        {
            RaceStarted = null;
            RaceFinished = null;
            DriversChanged = null;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs args)
        {
            RandomlyPunishParticipant();
            MoveParticipants();

            if (_needsUpdate)
            {
                DriversChangedEventArgs e = new DriversChangedEventArgs(Track);
                DriversChanged?.Invoke(this, e);
                _needsUpdate = false;
            }
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
            Participants.ForEach(p =>
            {
                p.Equipment.Performance = _random.Next(Car.MIN_PERFORMANCE, Car.MAX_PERFORMANCE);
                p.Equipment.Speed = _random.Next(Car.MIN_SPEED, Car.MAX_SPEED);

                if (p.Equipment.Performance * p.Equipment.Speed > ((Car.MAX_SPEED * Car.MAX_PERFORMANCE) - (Car.MIN_SPEED * Car.MIN_PERFORMANCE)) / 2 )
                    p.Equipment.Quality = _random.Next(Car.MIN_QUALITY, Car.MAX_PERFORMANCE / 2);
                else
                    p.Equipment.Quality = _random.Next(Car.MAX_PERFORMANCE / 2, Car.MAX_PERFORMANCE);
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

        public void RandomlyPunishParticipant()
        {
            foreach(IParticipant p in Participants)
            {
                if (p.Equipment.IsBroken)
                {
                    if (_random.Next(1, 30 / p.Equipment.Quality) == 1)
                    {
                        _needsUpdate = true;
                        p.Equipment.IsBroken = false;
                        if (p.Equipment.Performance > Car.MIN_PERFORMANCE)
                            p.Equipment.Performance -= 1;
                    }
                } else
                    if (_random.Next(1, p.Equipment.Quality) == 1)
                    {
                        _needsUpdate = true;
                        p.Equipment.IsBroken = true;
                    }
            }
        }

        public void InitializeLaps()
        {
            ParticipantLaps = new Dictionary<IParticipant, int>();

            foreach (IParticipant p in Participants)
                ParticipantLaps.Add(p, 0);
        }

        public void AddParticipantLap(IParticipant p)
        {
            ParticipantLaps[p] += 1;
            if (ParticipantLaps[p] >= Laps)
                ParticipantFinished(p);

            TimeSpan elapsed = DateTime.Now - _startTime;
            if (LapTimes.TryGetValue(p, out LinkedList<TimeSpan> times))
            {
                TimeSpan time = elapsed - times.Last.Value;
                times.AddLast(time);
            } 
            else
            {
                LinkedList<TimeSpan> list = new LinkedList<TimeSpan>();
                list.AddLast(elapsed);
                LapTimes.Add(p, list);
            }
        }

        public void ParticipantFinished(IParticipant p)
        {
            SectionData data = GetSectionData(Track.Sections.First.Value);
            if (data.Left == p)
                data.Left = null;
            if (data.Right == p)
                data.Right = null;

            Leaderboard.AddLast(p);
            p.Points += (4 - Leaderboard.Count < 1 ? 1 : 4 - Leaderboard.Count);
            CheckFinished();
        }

        public void CheckFinished()
        {
            if (Leaderboard.Count == Participants.Count)
                Stop();
        }

        public void MoveParticipants()
        {
            foreach (Section section in Track.Sections)
            {
                SectionData data = GetSectionData(section);
                int distance;
                if (data.Left != null && !data.Left.Equipment.IsBroken)
                {
                    distance = data.Left.Equipment.Performance * data.Left.Equipment.Speed;
                    data.DistanceLeft += distance;
                }

                if (data.Right != null && !data.Right.Equipment.IsBroken)
                {
                    distance = data.Right.Equipment.Performance * data.Right.Equipment.Speed;
                    data.DistanceRight += distance;
                }

                if (data.DistanceLeft >= Section.Length && data.DistanceRight >= Section.Length)
                {
                    if (data.DistanceLeft >= data.DistanceRight)
                    {
                        MoveToNext(section, data.Left);
                        MoveToNext(section, data.Right);
                    }
                    else
                    {
                        MoveToNext(section, data.Right);
                        MoveToNext(section, data.Left);
                    }
                } else
                {
                    if (data.DistanceLeft >= Section.Length)
                        MoveToNext(section, data.Left);

                    if (data.DistanceRight >= Section.Length)
                        MoveToNext(section, data.Right);
                }
            }

            CheckAllWaiting();
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
