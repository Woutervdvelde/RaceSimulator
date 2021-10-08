﻿using System;
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

            _race = new Race(_track, _participants, 1);
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
        public void MoveParticipants_AddToWaitingWhen100Added()
        {
            foreach (Driver d in _race.Participants)
                d.Equipment.Performance = d.Equipment.Speed = 10;

            _race.MoveParticipants();

            SectionData data = _race.GetSectionData(_race.Track.Sections.ElementAt(2));
            Assert.AreEqual(2, data.Waiting.Count);
        }

        [Test]
        public void MoveParticipants_MoveToNextSectionWhenNonWaiting()
        {
            foreach (Driver d in _race.Participants)
                d.Equipment.Performance = d.Equipment.Speed = 10;

            _race.MoveParticipants();

            SectionData data = _race.GetSectionData(_race.Track.Sections.ElementAt(3));
            Assert.AreNotEqual(null, data.Left);
            Assert.AreNotEqual(null, data.Right);
        }

        [Test]
        public void MoveParticipants_AddToWaitingOnStartWhenOnFinish()
        {
            Track track = new Track("UnitTrack", new[] { SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Finish });
            List<IParticipant> participants = new List<IParticipant>();
            participants.Add(new Driver("1", new Car(), TeamColors.Blue));
            Race race = new Race(_track, _participants, 1);

            race.Participants[0].Equipment.Performance = 10;
            race.Participants[0].Equipment.Speed = 10;

            SectionData finish = race.GetSectionData(race.Track.Sections.Last.Value);
            finish.Left = race.Participants[0];

            race.MoveParticipants();

            SectionData start = race.GetSectionData(race.Track.Sections.First.Value);
            Assert.AreEqual(race.Participants[0], start.Waiting.Dequeue());
        }
    }
}
