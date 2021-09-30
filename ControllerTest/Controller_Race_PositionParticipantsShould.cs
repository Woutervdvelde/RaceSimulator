using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Model;

namespace ControllerTest
{
    [TestFixture]

    class Controller_Race_PositionParticipantsShould
    {
        public Race Race;
        public Track Track;
        public List<IParticipant> Participants;

        [SetUp]
        public void SetUp()
        {
            Track = new Track("UnitTrack", new[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Finish });

            Participants = new List<IParticipant>();
            Participants.Add(new Driver("1", new Car(), TeamColors.Blue));
            Participants.Add(new Driver("2", new Car(), TeamColors.Blue));
            Participants.Add(new Driver("3", new Car(), TeamColors.Blue));

            Race = new Race(Track, Participants);
        }

        [Test]
        public void PositionParticipants_PlaceThreeParticpantsInFront()
        {
            Race.PositionParticipants(Track, Participants);

            SectionData data = Race.GetSectionData(Race.Track.Sections.ElementAt(2));
            Assert.AreEqual(data.Left.Name, Participants[0].Name);
            Assert.AreEqual(data.Right.Name, Participants[1].Name);

            data = Race.GetSectionData(Race.Track.Sections.ElementAt(1));
            Assert.AreEqual(data.Left.Name, Participants[2].Name);
            Assert.IsNull(data.Right);
        }

        [Test]
        public void PositionParticipants_PlaceThree_ShouldNotInBack()
        {
            Race.PositionParticipants(Track, Participants);

            SectionData data = Race.GetSectionData(Race.Track.Sections.ElementAt(0));
            Assert.IsNull(data.Left);
            Assert.IsNull(data.Right);

            data = Race.GetSectionData(Race.Track.Sections.ElementAt(0));
            Assert.IsNull(data.Right);
        }

        [Test]
        public void PositionParticipants_ExceptionTooManyParticipants()
        {
            Participants.Add(new Driver("4", new Car(), TeamColors.Blue));
            Participants.Add(new Driver("5", new Car(), TeamColors.Blue));
            Participants.Add(new Driver("6", new Car(), TeamColors.Blue));
            Participants.Add(new Driver("7", new Car(), TeamColors.Blue));
            Participants.Add(new Driver("8", new Car(), TeamColors.Blue));
            Participants.Add(new Driver("9", new Car(), TeamColors.Blue));

            Assert.Throws<Exception>(() => { Race.PositionParticipants(Track, Participants);});
        }

        [Test]
        public void PositionParticipants_ExceptionTooManyParticipants_WithNoStartGrids()
        {
            Track track = new Track("UnitTrack", new[] { SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Finish });
            Assert.Throws<Exception>(() => { Race.PositionParticipants(track, Participants); });
        }
    }
}
