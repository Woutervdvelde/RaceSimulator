using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Model;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            Track result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Track track = new Track("UnitTrack", new[] { SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Finish });
            _competition.Tracks.Enqueue(track);

            Track result = _competition.NextTrack();
            Assert.AreEqual(track, result);
        }

        [Test]
        public void NextTrack_OneInQue_RemoveTrackFromQueue()
        {
            Track track = new Track("UnitTrack", new[] { SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Finish });
            _competition.Tracks.Enqueue(track);

            Track result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Track track1 = new Track("UnitTrack", new[] { SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Finish });
            Track track2 = new Track("UnitTrack", new[] { SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Finish });
            _competition.Tracks.Enqueue(track1);
            _competition.Tracks.Enqueue(track2);

            Track result = _competition.NextTrack();
            Assert.AreEqual(track1, result);

            result = _competition.NextTrack();
            Assert.AreEqual(track2, result);
        }
    }
}
