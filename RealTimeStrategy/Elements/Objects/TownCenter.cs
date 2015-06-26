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
    class TownCenter : GameObject
    {
        public TownCenter(string _name, string _type, Vector2 _position)
            : base(_name, _type, _position)
        {
            hotKeys = new List<Keys>();
            hotKeys.Add(Keys.V);
            target = new Vector2(_position.X - 25, _position.Y - 25);
            selected = false;
            position = _position;
            type = _type;
            name = _name;
            texture = Game1.ASM.GetTextureAsset(name);
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.sprite.Width, texture.sprite.Height);
        }
        public override void RightClickAction(Vector2 mousePos)
        {
            target = new Vector2((mousePos.X + Game1.CAM.offset.X - 12),
                (mousePos.Y + Game1.CAM.offset.Y - 12));
        }
        public override void Update()
        {
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
        }
        public override void HotkeyAction(Keys key)
        {
            switch (key)
            {
                case Keys.V:
                    QueueVillager();
                    break;
                default:
                    break;
            }
        }
        public void QueueVillager()
        {
            Objects.Villager tempVillager = new Objects.Villager("Villager", "Unit",                 
                new Vector2
                    (position.X + (texture.sprite.Width / 2),
                     position.Y + (texture.sprite.Height / 2)));
            Game1.OBM.AddObject("Unit", tempVillager);
            tempVillager.GoTo(new Vector2(target.X, target.Y),new Vector2(position.X - Game1.CAM.offset.X + (texture.sprite.Width / 2),
                                    position.Y - Game1.CAM.offset.Y + (texture.sprite.Height / 2)));
            
        }
    }
}
