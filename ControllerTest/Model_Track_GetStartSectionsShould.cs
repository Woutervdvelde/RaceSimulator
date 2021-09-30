using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace ControllerTest
{
    [TestFixture]
    class Model_Track_GetStartSectionsShould
    {
        private Track Track;

        [SetUp]
        public void SetUp()
        {
            Track = new Track("UnitTrack", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.Finish });
        }

        [Test]
        public void GetStartSections_NoStartGrids_ReturnEmpty()
        {
            Track noStartTrack = new Track("NoStart", new SectionTypes[] { SectionTypes.Straight, SectionTypes.Finish });
            LinkedList<Section> startSections = noStartTrack.GetStartSections();
            Assert.AreEqual(0, startSections.Count);
        }

        [Test]
        public void GetStartSections_ThreeStartGrids_ReturnThree()
        {
            LinkedList<Section> startSections = Track.GetStartSections();
            Assert.AreEqual(3, startSections.Count);
            foreach (Section section in startSections)
            {
                Assert.AreEqual(SectionTypes.StartGrid, section.SectionType);
            }
        }

        [Test]
        public void GetStartSections_OnlyStartGrids_ReturnAll()
        {
            Track allStartTrack = new Track("AllStart", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid });
            LinkedList<Section> startSections = allStartTrack.GetStartSections();

            Assert.AreEqual(4, startSections.Count);
            foreach (Section section in startSections)
            {
                Assert.AreEqual(SectionTypes.StartGrid, section.SectionType);
            }
        }
    }
}
