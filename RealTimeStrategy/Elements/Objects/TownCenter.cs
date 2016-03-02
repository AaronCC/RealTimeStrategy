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
    class TownCenter : Structure
    {
        public TownCenter(string _name, string _type, Vector2 _position)
            : base(_name, _type, _position)
        {
            buildTime = 10000;
            building = false;
            currentLoadTime = 0;
            hotKeyImages = new List<KeyValuePair<char, Assets.TextureAsset>>();
            hotKeys = new List<Keys>();
            AddHotkey(Keys.V, "Villager", 'V');
            target = new Point((int)_position.X - 25, (int)_position.Y - 25);
            selected = false;
            position = _position;
            type = _type;
            name = _name;
            texture = Game1.ASM.GetTextureAsset(name);
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.sprite.Width, texture.sprite.Height);
        }

        public override void RightClickAction(Vector2 mousePos)
        {
            Point chunkLoc = Game1.OBM.CalcChunkIndex(new Point(((int)(mousePos.X + Game1.CAM.offset.X)),
                ((int)(mousePos.Y + Game1.CAM.offset.Y))));
            target = new Point(chunkLoc.X * 25, chunkLoc.Y * 25);
        }
        public override void Initialize()
        {
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.sprite.Width, texture.sprite.Height);
            target = new Point((int)position.X - 25, (int)position.Y - 25);
            Game1.OBM.AddObject("Structure", this);
            PushUpdate();
        }
        public override void Update(GameTime gameTime)
        {
            updated = false;
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
            if (loadQueue.Count > 0)
            {
                if (currentLoadTime == 0)
                {
                    currentLoadTime = loadQueue.Peek().Key;
                }
                currentLoadTime -= gameTime.ElapsedGameTime.Milliseconds;
                PushUpdate();
                if (currentLoadTime <= 0)
                {
                    currentLoadTime = 0;
                    if (loadQueue.Peek().Value as Objects.Unit != null)
                    {
                        Unit unit = (Objects.Unit)loadQueue.Peek().Value;
                        unit.Initialize(new Point(target.X, target.Y), new Vector2(position.X - Game1.CAM.offset.X + (texture.sprite.Width),
                                    position.Y - Game1.CAM.offset.Y + (texture.sprite.Height)));
                    }
                    loadQueue.Dequeue();
                }
            }
            if(building && workers.Count > 0)
            {
                int time = gameTime.ElapsedGameTime.Milliseconds;
                buildProgress += time * workers.Count;
                if (buildProgress >= buildTime)
                {
                    building = false;
                    int i = 0;
                    while(workers.Count > 0)
                    {
                        workers[i].StopBuilding();
                        i++;

                    }
                }
                else
                    PushUpdate();
            }
        }
        public override void HotkeyAction(Keys key)
        {
            if (!building)
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
        }
        public void QueueVillager()
        {
            Objects.Villager tempVillager = new Objects.Villager("Villager", "Unit",
                new Vector2
                    (position.X + (texture.sprite.Width),
                     position.Y + (texture.sprite.Height)));

            KeyValuePair<int, GameObject> villagerPair = new KeyValuePair<int, GameObject>(2000, tempVillager);
            if (loadQueue.Count < loadQueueCapacity)
                loadQueue.Enqueue(villagerPair);
            if(!updated)
            PushUpdate();
        }
    }
}
