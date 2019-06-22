using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class CameraTest
    {
        [TestMethod]
        public void Camera_PositionXTest()
        {
            Camera cam = new Camera();
            cam.PositionX = 1.1f;
            Assert.AreEqual(cam.PositionX, 1.1f);
        }

        [TestMethod]
        public void Camera_SetFocalPoint()
        {
            ScreenManager.Instance.Dimensions = new Vector2(500, 400);
            Camera cam = new Camera();
            cam.SetFocalPoint(new Vector2(10, 10));
            Assert.AreEqual(cam.mCameraPosition, new Vector2(0, 0));
            cam.SetFocalPoint(new Vector2(600, 300));
            Assert.AreEqual(cam.mCameraPosition, new Vector2(350, 100));
        }
    }
}
