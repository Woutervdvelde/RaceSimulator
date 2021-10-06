using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Model;

namespace ControllerTest
{
    [TestFixture]
    class Model_Track_GetPrevSectionShould
    {
        private Track _track;

        [SetUp]
        public void SetUp()
        {
            _track = new Track("UnitTrack", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Finish });
        }

        [Test]
        public void GetPrevSection_ReturnPrevSection()
        {
            Section search = _track.Sections.ElementAt(1);
            Section expected = _track.Sections.ElementAt(0);
            Section result = _track.GetPreviousSection(search);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetPrevSection_ReturnNotSameSection()
        {
            Section search = _track.Sections.ElementAt(1);
            Section expected = _track.Sections.ElementAt(1);
            Section result = _track.GetPreviousSection(search);

            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void GetPrevSection_ReturnNotNextNextSection()
        {
            Section search = _track.Sections.ElementAt(2);
            Section expected = _track.Sections.ElementAt(0);
            Section result = _track.GetPreviousSection(search);

            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void GetPrevSection_ReturnLastIfNotFound()
        {
            Section search = _track.Sections.ElementAt(0);
            Section expected = _track.Sections.ElementAt(2);
            Section result = _track.GetPreviousSection(search);

            Assert.AreEqual(expected, result);
        }

    }
}
