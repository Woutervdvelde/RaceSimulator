using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Model;

namespace ControllerTest
{
    [TestFixture]
    class Model_Track_GetNextSectionShould
    {
        private Track _track;
        [SetUp]
        public void SetUp()
        {
            _track = new Track("UnitTrack", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Finish });
        }

        [Test]
        public void GetNextSection_ReturnNextSection()
        {
            Section search = _track.Sections.ElementAt(0);
            Section expected = _track.Sections.ElementAt(1);
            Section result = _track.GetNextSection(search);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetNextSection_ReturnNotSameSection()
        {
            Section search = _track.Sections.ElementAt(0);
            Section expected = _track.Sections.ElementAt(0);
            Section result = _track.GetNextSection(search);

            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void GetNextSection_ReturnNotNextNextSection()
        {
            Section search = _track.Sections.ElementAt(0);
            Section expected = _track.Sections.ElementAt(2);
            Section result = _track.GetNextSection(search);

            Assert.AreNotEqual(expected, result);
        }

        [Test]
        public void GetNextSection_ReturnFirstIfNotFound()
        {
            Section search = _track.Sections.ElementAt(2);
            Section expected = _track.Sections.ElementAt(0);
            Section result = _track.GetNextSection(search);

            Assert.AreEqual(expected, result);
        }
    }
}
