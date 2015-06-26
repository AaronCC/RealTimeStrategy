#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion
namespace RealTimeStrategy.Managers
{
    public class ScreenManager
    {
        public Vector2 virtualScreen { get; set; }
        public Vector2 actualScreen { get; set; }
        public Vector3 scalingFactor { get; set; }
        public Matrix scale { get; set; }
        public ScreenManager(Vector2 _virtual, Vector2 _actual)
        {
            virtualScreen = _virtual;
            actualScreen = _actual;
        }
        public void CalcScaleFactor()
        {
            float widthScale = (float)actualScreen.X / virtualScreen.X;
            float heightScale = (float)actualScreen.Y / virtualScreen.Y;
            scalingFactor = new Vector3(widthScale, heightScale, 1);
            scale = Matrix.CreateScale(scalingFactor);
        }
    }
}
