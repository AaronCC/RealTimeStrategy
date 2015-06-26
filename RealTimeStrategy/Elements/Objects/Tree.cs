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

namespace RealTimeStrategy.Elements.Objects
{
    class Tree : GameObject
    {
        public Tree(string _name, string _type, Vector2 _position)
            : base(_name, _type, _position)
        {
            hotKeys = new List<Keys>();
            target = new Vector2(_position.X, _position.Y);
            selected = false;
            position = _position;
            type = _type;
            name = _name;
            texture = Game1.ASM.GetTextureAsset(name);
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.sprite.Width, texture.sprite.Height);
        }
    }
}
