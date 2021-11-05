using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using RaceSimulatorGUI;
using System.Drawing;

namespace ControllerTest
{
    [TestFixture]
    class RaceSimulatorGUI_ResourceManager_ClearShould
    {
        [SetUp]
        public void SetUp()
        {
            ResourceManager.Initialize();
        }

        [Test]
        public void Clear_CachedEmptyImageShouldBeCleared()
        {
            Bitmap a = ResourceManager.GetEmptyImage(100, 100);
            ResourceManager.Clear();
            Bitmap b = ResourceManager.GetEmptyImage(200, 200);

            Assert.AreEqual(200, b.Width);
            Assert.AreEqual(200, b.Height);
        }
    }
}
