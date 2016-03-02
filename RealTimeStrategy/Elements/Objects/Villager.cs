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
    class Villager : Unit
    {
        int i;
        private Objects.GameObject buildObject;
        bool buildSet, building;
        
        public Villager(string _name, string _type, Vector2 _position)
            : base(_name, _type, _position)
        {
            building = false;
            arrived = false;
            buildSet = false;
            hotKeyImages = new List<KeyValuePair<char, Assets.TextureAsset>>();
            hotKeys = new List<Keys>();
            AddHotkey(Keys.V, "TownCenter", 'V');
            i = 0;
            speed = 2;
        }

        public override void Update(GameTime gameTime)
        {
            updated = false;
            if (selected)
                CheckBuild();
            if (path.Count > 0)
            {
                Game1.OBM.GetWorld().UpdateObject(this);
                Game1.OBM.GetWorld().RemoveObject(oldHitBox, this);
                if (remaining <= 0)
                {
                    if (i == 1)
                        i = 0;
                    else
                    {
                        position.X = path.Peek().X * 25;
                        position.Y = path.Peek().Y * 25;
                    }
                    velocity = Vector2.Zero;
                    path.Pop();
                    if (path.Count == 0)
                    { 
                        Point posIndex = Game1.OBM.CalcChunkIndex(new Point((int)position.X,(int)position.Y));
                        arrived = true;
                        PushUpdate();
                    }
                    if (path.Count > 0)
                    {
                        CalcVelocity(path.Peek(), Game1.OBM.CalcChunkIndex(new Point((int)position.X, (int)position.Y)));
                        Point chunkPos = Game1.OBM.CalcChunkIndex(new Point((int)position.X, (int)position.Y));
                        remaining = Math.Abs(Math.Sqrt(Math.Pow((path.Peek().X * 25) - (chunkPos.X * 25), 2) + Math.Pow((path.Peek().Y * 25) - (chunkPos.Y * 25), 2)));
                    }
                }
                else
                {
                    position += velocity;
                    remaining -= speed;
                }
                Game1.OBM.GetWorld().InsertObject(hitBox, this);
            }
            oldHitBox = hitBox;
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
        }
        public override void Initialize(Point _target, Vector2 _start)
        {
            i = 1;
            Game1.OBM.AddObject("Unit", this);
            GoTo(_target, _start);
            if (path.Count > 0)
                CalcVelocity(path.Peek(), Game1.OBM.CalcChunkIndex(new Point((int)position.X, (int)position.Y)));
        }
        public override void RightClickAction(Vector2 mousePos)
        {
            StopBuilding();
            Point queryPoint = Game1.OBM.CalcChunkIndex(new Point((int)(mousePos.X + Game1.CAM.offset.X), (int)(mousePos.Y + Game1.CAM.offset.Y)));
            Rectangle queryRect = new Rectangle((int)mousePos.X, (int)mousePos.Y, hitBox.Width, hitBox.Height);
            List<GameObject> queryObjects = Game1.OBM.GetWorld().Query(queryPoint, queryRect);
            foreach(GameObject obj in queryObjects)
            {
                if (obj as Structure != null)
                {
                    buildObject = obj;
                    buildSet = true;
                    mousePos = obj.position - Game1.CAM.offset;
                    mousePos.X -= Game1.OBM.GetWorld().chunkSize;
                    mousePos.Y -= Game1.OBM.GetWorld().chunkSize;
                    break;
                }
            }
            i = 1;
            path.Clear();
            Game1.OBM.GetWorld().UpdateObject(this);
            Point chunkLoc = Game1.OBM.CalcChunkIndex(new Point(((int)(mousePos.X + Game1.CAM.offset.X)),
                ((int)(mousePos.Y + Game1.CAM.offset.Y))));
            GoTo(new Point(chunkLoc.X * 25, chunkLoc.Y * 25), position - Game1.CAM.offset);
        }
        public override void HotkeyAction(Keys key)
        {
            switch (key)
            {
                case Keys.V:
                    QueueTownCenter();
                    break;
                default:
                    break;
            }
        }
        public void QueueTownCenter()
        {
            StopBuilding();
            Point index = Game1.OBM.CalcChunkIndex(ToPoint(Game1.OBM.GetPlayer().mousePos.X + Game1.CAM.offset.X, Game1.OBM.GetPlayer().mousePos.Y + Game1.CAM.offset.Y));
            Objects.TownCenter tempCenter = new Objects.TownCenter("TownCenter", "Structure", new Vector2(index.X * Game1.OBM.GetWorld().chunkSize, index.Y * Game1.OBM.GetWorld().chunkSize));
            buildObject = tempCenter;
            buildSet = false;
                PushUpdate();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (buildObject as Structure != null)
                spriteBatch.Draw(buildObject.texture.sprite, buildObject.position - Game1.CAM.offset, Color.White * .5f);
            spriteBatch.Draw(texture.sprite, position - Game1.CAM.offset, Color.White);
        }
        public void CheckBuild()
        {
            // If there is a buildObject
            if(buildObject as Structure != null && !building)
            {
                Objects.Structure buildStructure = buildObject as Structure;
                if (selected && !buildSet)
                {
                    Point index = Game1.OBM.CalcChunkIndex(ToPoint(Game1.OBM.GetPlayer().mousePos.X + Game1.CAM.offset.X, Game1.OBM.GetPlayer().mousePos.Y + Game1.CAM.offset.Y));
                    buildObject.position = new Vector2(index.X * Game1.OBM.GetWorld().chunkSize, index.Y * Game1.OBM.GetWorld().chunkSize);
                }
                // If the build has not yet been set and left click
                if (Game1.OBM.GetPlayer().mState.LeftButton == ButtonState.Pressed && Game1.OBM.GetPlayer().old_mState.LeftButton == ButtonState.Released && !buildSet)
                {
                    buildSet = true;
                    GoTo(new Point((int)buildObject.position.X - Game1.OBM.GetWorld().chunkSize, (int)buildObject.position.Y - Game1.OBM.GetWorld().chunkSize), position);
                }
                // If the build has been set and the villager has arrived
                if(buildSet && arrived)
                {
                    BuildStructure(buildStructure);
                }
                    PushUpdate();
            }
        }
        public void BuildStructure(Structure buildStructure)
        {
            building = true;
            buildStructure.StartBuild();
            buildStructure.workers.Add(this);
            arrived = false;
            buildSet = false;
        }
        public void StopBuilding()
        {
            if (buildObject as Structure != null)
            {
                Structure buildStruct = buildObject as Structure;
                if (buildStruct.workers.Contains(this))
                    buildStruct.workers.Remove(this);
                building = false;
                buildObject = new Objects.GameObject();
            }
        }
    }

}
