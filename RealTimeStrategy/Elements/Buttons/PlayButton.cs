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
    public class PlayButton : Button
    {
        public PlayButton(Vector2 p, string n):base(p,n)
        {
            name = n;
            position = p;
            flagged = false;
            texture = Game1.ASM.GetTextureAsset(name);
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.sprite.Width, texture.sprite.Height);
        }
        public override void ClickEvent()
        {
            Game1.ASM.PlaySound("ButtonPress", false);
            Game1.OBM.AddMenu(Game1.menuDict["WorldSelectMenu"]);
        }
    }
}
