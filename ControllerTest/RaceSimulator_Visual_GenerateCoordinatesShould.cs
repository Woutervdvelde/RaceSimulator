using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Model;
using RaceSimulator;

namespace ControllerTest
{
    [TestFixture]
    class RaceSimulator_Visual_GenerateCoordinatesShould
    {
        private Track _t;
        [SetUp]
        public void SetUp()
        {
            /*
             * The following generates the following track
             * ╔ ═ ╗
             * 
             * ╚ ═ ╝
             * Where the left top corner (╔) has the coordinates: X = -1    Y = 0
             */
            _t = new Track("Test", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner });
        }

        [Test]
        public void GenerateCoordinates_GeneratePredictedCoordinates()
        {
            Visual.GenerateCoordinatesAndGraphics(_t);
            //first = X, second = Y
            int[,] prediction = new int[,] { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, 0 } };
            
            for (int i = 0; i < _t.Sections.Count; i++)
            {
                Assert.AreEqual(prediction[i,0], _t.Sections.ElementAt(i).X);
                Assert.AreEqual(prediction[i,1], _t.Sections.ElementAt(i).Y);
            }
        }
    }
}
