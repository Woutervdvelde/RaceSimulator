using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Text;
using System.Linq;
using Controller;
using Model;

namespace ControllerTest
{
    [TestFixture]
    class Controller_Race_MoveParticipantsShould
    {
        private Race _race;
        private Track _track;
        private List<IParticipant> _participants;

        /**
         * !IMPORTANT!
         * These tests have been tested with a Section length of 100
         */

        [SetUp]
        public void SetUp()
        {
            _track = new Track("UnitTrack", new[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Finish });

            _participants = new List<IParticipant>();
            _participants.Add(new Driver("1", new Car(), TeamColors.Blue));
            _participants.Add(new Driver("2", new Car(), TeamColors.Blue));
            _participants.Add(new Driver("3", new Car(), TeamColors.Blue));
            _participants.Add(new Driver("4", new Car(), TeamColors.Blue));

            _race = new Race(_track, _participants, 2);
            _race.PositionParticipants(_race.Track, _race.Participants);
        }

        [Test]
        public void MoveParticipants_Add50DistanceToAllParticipants()
        {
            foreach (Driver d in _race.Participants)
                d.Equipment.Performance = d.Equipment.Speed = 5;

            _race.MoveParticipants();
            foreach(Section s in _race.Track.Sections)
            {
                SectionData data = _race.GetSectionData(s);
                if (data.Left != null)
                    Assert.AreEqual(25, data.DistanceLeft);
                if (data.Right != null)
                    Assert.AreEqual(25, data.DistanceRight);
            }
        }

        [Test]
        public void MoveParticipants_MoveToNextSectionWhenNonOccupied()
        {
            foreach (Driver d in _race.Participants)
                d.Equipment.Performance = d.Equipment.Speed = 10;

            _race.MoveParticipants();

            SectionData data = _race.GetSectionData(_race.Track.Sections.ElementAt(3));
            Assert.AreNotEqual(null, data.Left);
            Assert.AreNotEqual(null, data.Right);
        }

        [Test]
        public void MoveParticipants_AddToWaiting_WhenDistanceLongerThanSectionLength_AndNextOccupied()
        {
            _race.Participants[0].Equipment.Performance = _race.Participants[1].Equipment.Performance = 1;
            _race.Participants[0].Equipment.Speed = _race.Participants[1].Equipment.Speed = 1;
            _race.Participants[2].Equipment.Performance = _race.Participants[3].Equipment.Performance = 10;
            _race.Participants[2].Equipment.Speed = _race.Participants[3].Equipment.Speed = 10;

            _race.MoveParticipants();

            SectionData data = _race.GetSectionData(_race.Track.Sections.ElementAt(2));
            Assert.AreEqual(2, data.Waiting.Count);
        }

        [Test]
        public void MoveParticipants_MoveToStartWhenOnFinish()
        {
            _race.Participants[0].Equipment.Performance = 10;
            _race.Participants[0].Equipment.Speed = 10;

            SectionData start = _race.GetSectionData(_race.Track.Sections.First.Value);
            SectionData finish = _race.GetSectionData(_race.Track.Sections.Last.Value);

            finish.Left = _race.Participants[0];
            _race.MoveParticipants();

            Assert.AreEqual(true, start.Left != null || start.Right != null);
            if (start.Left != null)
                Assert.AreEqual(_race.Participants[0], start.Left);
            if (start.Right != null)
                Assert.AreEqual(_race.Participants[0], start.Right);
        }

        [Test]
        public void MoveParticipants_MoveFastestFirst()
        {
            _race.Participants[0].Equipment.Performance = _race.Participants[1].Equipment.Performance = 1;
            _race.Participants[0].Equipment.Speed = _race.Participants[1].Equipment.Speed = 1;
            _race.Participants[2].Equipment.Performance = 11;
            _race.Participants[2].Equipment.Speed = 11;
            _race.Participants[3].Equipment.Performance = 12;
            _race.Participants[3].Equipment.Speed = 12;

            SectionData s = _race.GetSectionData(_race.Track.Sections.ElementAt(2));

            _race.MoveParticipants();

            Assert.AreEqual(_race.Participants[3], s.Waiting.Dequeue());
            Assert.AreEqual(_race.Participants[2], s.Waiting.Dequeue());
        }
    }
}
