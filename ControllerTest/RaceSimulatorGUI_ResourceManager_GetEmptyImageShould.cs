using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using RaceSimulatorGUI;
using System.Drawing;

namespace ControllerTest
{
    [TestFixture]
    class RaceSimulatorGUI_ResourceManager_GetEmptyImageShould
    {
        [SetUp]
        public void SetUp()
        {
            ResourceManager.Initialize();
        }

        [Test]
        public void GetEmptyImage_ReturnEmptyImageWithGivenDimensions()
        {
            int width = 100;
            int height = 100;
            Bitmap b = ResourceManager.GetEmptyImage(100, 100);
            Assert.AreEqual(width, b.Width);
            Assert.AreEqual(height, b.Height);
        }

        [Test]
        public void GetEmptyImage_ReturnUnaffectedCachedEmptyImage()
        {
            Bitmap a = ResourceManager.GetEmptyImage(100, 100);
            Bitmap b = ResourceManager.GetEmptyImage(200, 200);

            Assert.AreEqual(100, b.Width);
            Assert.AreEqual(100, b.Height);
        }
    }
}
