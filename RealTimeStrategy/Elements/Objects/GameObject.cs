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
        public Point target;
        public List<Keys> hotKeys;
        public List<KeyValuePair<char, Assets.TextureAsset>> hotKeyImages;
        public bool updated, drawn;
        public GameObject() { }
        public GameObject(string _name, string _type, Vector2 _position)
        {
            drawn = false;
            updated = false;
            hotKeys = new List<Keys>();
            target = new Point((int)_position.X, (int)_position.Y);
            selected = false;
            position = _position;
            type = _type;
            name = _name;
            texture = Game1.ASM.GetTextureAsset(name);
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.sprite.Width, texture.sprite.Height);
        }
        public virtual void Update(GameTime gameTime)
        {
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture.sprite, position - Game1.CAM.offset, Color.White);
        }
        public void AddHotkey(Keys key, string texture, char charKey)
        {
            hotKeys.Add(key);
            hotKeyImages.Add(new KeyValuePair<char, Assets.TextureAsset>(charKey, Game1.ASM.GetTextureAsset(texture)));
        }

        public virtual void RightClickAction(Vector2 mousePos) { }
        public virtual void HotkeyAction(Keys key) { }
        public virtual void Initialize() { }

        public void PushUpdate()
        {
            if (!updated)
            {
                updated = true;

                Game1.OBM.PushUpdate(this.name);
            }
        }
        public Point ToPoint(float x, float y)
        {
            return new Point((int)x, (int)y);
        }
    }
}
