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
        public DateTime StartTime { get; set; }
        public event EventHandler DriversChanged;

        private Random _random;
        private Dictionary<Section, SectionData> _positions;
        private Timer _timer;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            _random = new Random(DateTime.Now.Millisecond);
            _positions = new Dictionary<Section, SectionData>();
            _timer = new Timer(500);
            _timer.Elapsed += OnTimedEvent;
            PositionParticipants(track, participants);
            RandomizeEquipment();
        }

        public void Start()
        {
            _timer.Start();
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
                _participant.Equipment.Quality = _random.Next(1, 10);
                _participant.Equipment.Performance = _random.Next(1, 10);
                _participant.Equipment.Speed = _random.Next(1, 10);
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
                    data.Left = participants[i];
                else
                    data.Right = participants[i];
            }
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

                if (data.DistanceLeft > Section.Length)
                    MoveToNext(section, data.Left);

                if (data.DistanceRight > Section.Length)
                    MoveToNext(section, data.Right);

                if (data.Waiting.Count > 0)
                    CheckWaiting(section, data);
            }
        }

        public void MoveToNext(Section currentSection, IParticipant participant)
        {
            Section nextSection = Track.GetNextSection(currentSection);
            SectionData nextData = GetSectionData(nextSection);
            nextData.Waiting.Enqueue(participant);
        }

        public void CheckWaiting(Section currentSection, SectionData data)
        {
            Section prevSection = Track.GetPreviousSection(currentSection);
            SectionData prevData = GetSectionData(prevSection);

            if (data.Left != null && data.Right != null)
                return;

            if (data.Waiting.Count < 2)
            {
                data.Waiting.TryDequeue(out IParticipant p);
                if (data.Left == null)
                    data.Left = p;
                if (data.Right == null)
                    data.Right = p;

                if (prevData.Left == p)
                {
                    prevData.Left = null;
                    prevData.DistanceLeft = 0;
                }
                if (prevData.Right == p) 
                {
                    prevData.Right = null;
                    prevData.DistanceRight = 0;
                }
            }

            DriversChangedEventArgs args = new DriversChangedEventArgs(Track);
            DriversChanged.Invoke(this, args);
        }
    }
}
