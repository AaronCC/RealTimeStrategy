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
    public class GameObject
    {
        public Assets.TextureAsset texture;
        public string name;
        public string type;
        public Vector2 position;
        public Rectangle hitBox;
        public bool selected;
        public Vector2 target;
        public List<Keys> hotKeys;
        public GameObject() { }
        public GameObject(string _name, string _type, Vector2 _position)
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
        public virtual void Update()
        {
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture.sprite, position - Game1.CAM.offset, Color.White);
        }
        public virtual void RightClickAction(Vector2 mousePos) { }
        public virtual void HotkeyAction(Keys key) { }
    }
}
