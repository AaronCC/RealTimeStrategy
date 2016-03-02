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
    class Structure : GameObject
    {
        public int loadQueueCapacity;
        public int currentLoadTime;
        public bool building;
        public int buildTime, buildProgress;
        public Queue<KeyValuePair<int, GameObject>> loadQueue;
        public List<Villager> workers;
        Texture2D loadBar, loadBack;
        Rectangle loadBarRect, loadBackRect;
        public Structure(string _name, string _type, Vector2 _position)
            : base(_name, _type, _position)
        {
            loadBarRect = new Rectangle(0, 0, 0, 10);
            loadBackRect = new Rectangle(0, 0, 0, 10);
            loadBack = Game1.ASM.GetTextureAsset("RedChunk").sprite;
            loadBar = Game1.ASM.GetTextureAsset("LoadBar").sprite;
            workers = new List<Villager>() ;
            loadQueueCapacity = 5;
            loadQueue = new Queue<KeyValuePair<int,GameObject>>();
            hotKeys = new List<Keys>();
            target = new Point((int)_position.X, (int)_position.Y);
            selected = false;
            position = _position;
            type = _type;
            name = _name;
            texture = Game1.ASM.GetTextureAsset(name);
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.sprite.Width, texture.sprite.Height);
        }
        public virtual void Initialize(Point _target, Vector2 _start)
        {
            Game1.OBM.AddObject("Unit", this);
        }
        public virtual void queueLoad(int time, GameObject obj)
        {
            KeyValuePair<int, GameObject> loadPair = new KeyValuePair<int, GameObject>(time, obj);
            loadQueue.Enqueue(loadPair);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(building)
            {
                double percent = (buildProgress / Convert.ToDouble(buildTime));
                loadBackRect.Width = (int)(texture.sprite.Width);
                loadBarRect.Width = (int)(texture.sprite.Width * percent);
                Rectangle DrawRect = new Rectangle(loadBarRect.X - (int)Game1.CAM.offset.X, loadBarRect.Y - (int)Game1.CAM.offset.Y, loadBarRect.Width, loadBarRect.Height);
                Rectangle BackDrawRect = new Rectangle(loadBackRect.X - (int)Game1.CAM.offset.X, loadBackRect.Y - (int)Game1.CAM.offset.Y, loadBackRect.Width, loadBackRect.Height);
                spriteBatch.Draw(loadBack, BackDrawRect, Color.White);
                spriteBatch.Draw(loadBar, DrawRect, Color.White);
                spriteBatch.Draw(texture.sprite, position - Game1.CAM.offset, Color.White * .5f);
            }
            else
                spriteBatch.Draw(texture.sprite, position - Game1.CAM.offset, Color.White);
        }
        public void StartBuild()
        {
            building = true;
            loadBarRect.X = (int)(position.X);
            loadBarRect.Y = (int)(position.Y) - 10;
            loadBackRect.X = (int)(position.X);
            loadBackRect.Y = (int)(position.Y) - 10;
            Initialize();
        }

    }
}
