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
        private Race _race;
        private Track _track;
        private List<IParticipant> _participants;

        [SetUp]
        public void SetUp()
        {
            _track = new Track("UnitTrack", new[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Finish });

            _participants = new List<IParticipant>();
            _participants.Add(new Driver("1", new Car(), TeamColors.Blue));
            _participants.Add(new Driver("2", new Car(), TeamColors.Blue));
            _participants.Add(new Driver("3", new Car(), TeamColors.Blue));

            _race = new Race(_track, _participants);
        }

        [Test]
        public void PositionParticipants_PlaceThreeParticpantsInFront()
        {
            _race.PositionParticipants(_track, _participants);

            SectionData data = _race.GetSectionData(_race.Track.Sections.ElementAt(2));
            Assert.AreEqual(data.Left.Name, _participants[0].Name);
            Assert.AreEqual(data.Right.Name, _participants[1].Name);

            data = _race.GetSectionData(_race.Track.Sections.ElementAt(1));
            Assert.AreEqual(data.Left.Name, _participants[2].Name);
            Assert.IsNull(data.Right);
        }

        [Test]
        public void PositionParticipants_PlaceThree_ShouldNotInBack()
        {
            _race.PositionParticipants(_track, _participants);

            SectionData data = _race.GetSectionData(_race.Track.Sections.ElementAt(0));
            Assert.IsNull(data.Left);
            Assert.IsNull(data.Right);

            data = _race.GetSectionData(_race.Track.Sections.ElementAt(0));
            Assert.IsNull(data.Right);
        }

        [Test]
        public void PositionParticipants_ExceptionTooManyParticipants()
        {
            _participants.Add(new Driver("4", new Car(), TeamColors.Blue));
            _participants.Add(new Driver("5", new Car(), TeamColors.Blue));
            _participants.Add(new Driver("6", new Car(), TeamColors.Blue));
            _participants.Add(new Driver("7", new Car(), TeamColors.Blue));
            _participants.Add(new Driver("8", new Car(), TeamColors.Blue));
            _participants.Add(new Driver("9", new Car(), TeamColors.Blue));

            Assert.Throws<Exception>(() => { _race.PositionParticipants(_track, _participants);});
        }

        [Test]
        public void PositionParticipants_ExceptionTooManyParticipants_WithNoStartGrids()
        {
            Track track = new Track("UnitTrack", new[] { SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Finish });
            Assert.Throws<Exception>(() => { _race.PositionParticipants(track, _participants); });
        }
    }
}
