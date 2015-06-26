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

namespace RealTimeStrategy.Elements.Buttons
{
    public class Button
    {
        public bool flagged;

        public Vector2 position;

        public Rectangle hitBox;

        public string name;

        public Assets.TextureAsset texture;

        public Button(Vector2 p, string n)
        {
            name = n;
            position = p;
            flagged = false ;
            texture = Game1.ASM.GetTextureAsset(name);
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.sprite.Width, texture.sprite.Height);
        }
        public virtual void ClickEvent()
        {

        }

    }
}
