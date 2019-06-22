using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorldXna
{
    public class Camera
    {
        private static Camera instance;
        public Vector2 mCameraPosition;
        
        public float PositionX
        {
            get { return mCameraPosition.X; }
            set { mCameraPosition.X = value; }
        }

        public Matrix ViewMatrix
        {
            get; private set;
        }

        public static Camera Instance
        {
            get
            {
                if (instance == null)
                    instance = new Camera();
                return instance;
            }
        }

        /// <summary>
        /// Een methode waarmee je de camera positie kan aanpassen aan een gewenste speler of iets anders
        /// </summary>
        /// <param name="focalPosition"></param>
        public void SetFocalPoint(Vector2 focalPosition)
        {
            mCameraPosition = new Vector2(focalPosition.X - (ScreenManager.Instance.Dimensions.X / 2), 
                focalPosition.Y - (ScreenManager.Instance.Dimensions.Y / 2));
            if (mCameraPosition.X < 0)
                mCameraPosition.X = 0;
            if (mCameraPosition.Y < 0)
                mCameraPosition.Y = 0;
        }

        public void Update()
        {
            ViewMatrix = Matrix.CreateTranslation(new Vector3(-mCameraPosition, 0));
        }
    }
}
