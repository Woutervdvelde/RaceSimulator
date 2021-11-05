using System.Collections.Generic;
using NUnit.Framework;
using System.Drawing;
using System.Text;
using System;
using Model;
using RaceSimulatorGUI;

namespace ControllerTest
{
    [TestFixture]
    class RaceSimulatorGUI_ResourceManager_GetDriverImageShould
    {
        [SetUp]
        public void SetUp()
        {
            ResourceManager.Initialize();
        }

        [Test]
        public void GetDriverImage_ReturnCorrectDriverImageCorner()
        {
            /* 
             * The following imageURL should be created "Blue_Corner_NE_East_Left"
             * The participant and track should exist out of:
             * TeamColor: Blue
             * SectionType: CornerRight
             * SectionDirection: NE
             * Direction: East
             * SectionSide: Left
             */

            SectionSides side = SectionSides.Left;
            TeamColors color = TeamColors.Blue;
            Section section = new Section(SectionTypes.RightCorner) { 
                SectionDirection = SectionDirections.NE,
                Direction = Directions.East
            };

            Bitmap a = ResourceManager.GetDriverImage(side, color, section);
            Bitmap b = ResourceManager.GetImage(".\\Resources\\Blue_Corner_NE_East_Left.png");

            Assert.AreEqual(b, a);
        }

        [Test]
        public void GetDriverImage_ReturnCorrectDriverImageHorizontalStraight()
        {
            /* 
             * The following imageURL should be created "Blue_Horizontal_Straight_East_Left"
             * The participant and track should exist out of:
             * TeamColor: Blue
             * SectionType: Straight
             * Direction: East
             * SectionSide: Left
             */

            SectionSides side = SectionSides.Left;
            TeamColors color = TeamColors.Blue;
            Section section = new Section(SectionTypes.Straight)
            {
                Direction = Directions.East
            };

            Bitmap a = ResourceManager.GetDriverImage(side, color, section);
            Bitmap b = ResourceManager.GetImage(".\\Resources\\Blue_Horizontal_Straight_East_Left.png");

            Assert.AreEqual(b, a);
        }

        [Test]
        public void GetDriverImage_ReturnCorrectDriverImageVerticalStraight()
        {
            /* 
             * The following imageURL should be created "Blue_Vertical_Straight_South_Left"
             * The participant and track should exist out of:
             * TeamColor: Blue
             * SectionType: Straight
             * Direction: South
             * SectionSide: Left
             */

            SectionSides side = SectionSides.Left;
            TeamColors color = TeamColors.Blue;
            Section section = new Section(SectionTypes.Straight)
            {
                Direction = Directions.South
            };

            Bitmap a = ResourceManager.GetDriverImage(side, color, section);
            Bitmap b = ResourceManager.GetImage(".\\Resources\\Blue_Vertical_Straight_South_Left.png");

            Assert.AreEqual(b, a);
        }

        [Test]
        public void GetDriverImage_NotReturnWrongColor()
        {
            /* 
             * The following imageURL should be created "Blue_Vertical_Straight_South_Left"
             * The participant and track should exist out of:
             * TeamColor: Blue
             * SectionType: Straight
             * Direction: South
             * SectionSide: Left
             */

            SectionSides side = SectionSides.Left;
            TeamColors color = TeamColors.Blue;
            Section section = new Section(SectionTypes.Straight)
            {
                Direction = Directions.South
            };

            Bitmap a = ResourceManager.GetDriverImage(side, color, section);
            //Green color filled in stead of Blue
            Bitmap b = ResourceManager.GetImage(".\\Resources\\Green_Vertical_Straight_South_Left.png");

            Assert.AreNotEqual(b, a);
        }
    }
}
