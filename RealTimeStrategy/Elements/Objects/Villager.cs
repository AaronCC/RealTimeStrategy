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
    class Villager : GameObject
    {
        public int speed;
        public Vector2 velocity, direction;
        public Queue<Vector2> wayPoints;
        public Rectangle oldHitBox;
        double ratio;
        public Villager(string _name, string _type, Vector2 _position)
            : base(_name, _type, _position)
        {
            speed = 2;
            wayPoints = new Queue<Vector2>();
            velocity = new Vector2(0, 0);
            hotKeys = new List<Keys>();
            target = new Vector2(_position.X, _position.Y);
            selected = false;
            position = _position;
            type = _type;
            name = _name;
            texture = Game1.ASM.GetTextureAsset(name);
            hitBox = new Rectangle((int)position.X, (int)position.Y, texture.sprite.Width, texture.sprite.Height);
            oldHitBox = hitBox;
        }
        public void GoTo(Vector2 _target, Vector2 start)
        {
            Vector2 targetVector;
            wayPoints.Enqueue(_target);
            target = _target;
            targetVector = (wayPoints.Peek() - Game1.CAM.offset) - start;
            double targetVectorMagnitude = (Math.Sqrt(((Math.Pow(targetVector.X, 2)) + (Math.Pow(targetVector.Y, 2)))));
            direction.X = (float)(targetVector.X * (1 / targetVectorMagnitude));
            direction.Y = (float)(targetVector.Y * (1 / targetVectorMagnitude));
            velocity = speed * direction;
        }
        public override void Update()
        {
            if (wayPoints.Count > 0)
            {
                Game1.OBM.GetWorld().UpdateObject(this);
                Game1.OBM.GetWorld().RemoveObject(oldHitBox, this);
                double remaining = Math.Abs(Math.Sqrt(Math.Pow(wayPoints.Peek().X - position.X, 2) + Math.Pow(wayPoints.Peek().Y - position.Y, 2)));
                if (remaining < speed)
                {
                    position.X = wayPoints.Peek().X;
                    position.Y = wayPoints.Peek().Y;
                    velocity = Vector2.Zero;
                    wayPoints.Dequeue();
                    if (wayPoints.Count > 0)
                        GoTo(wayPoints.Dequeue(), position - Game1.CAM.offset);
                }
                else
                {
                    //if (!CheckCollisions())
                        position += velocity;
                }
                Game1.OBM.GetWorld().InsertObject(hitBox, this);
            }
            oldHitBox = hitBox;
            hitBox.X = (int)position.X;
            hitBox.Y = (int)position.Y;
        }
        public bool CheckCollisions()
        {
            if (Game1.OBM.GetWorld().Query(Game1.OBM.CalcChunkIndex(new Point((int)(position.X + velocity.X), (int)(position.Y + velocity.Y))),
                       new Rectangle((int)(position.X + velocity.X), (int)(position.Y + velocity.Y), texture.sprite.Width, texture.sprite.Height)).Count == 0)
                return false;
            return true;
        }
        public override void RightClickAction(Vector2 mousePos)
        {
            wayPoints.Clear();
            Game1.OBM.GetWorld().UpdateObject(this);
            GoTo(new Vector2((mousePos.X + Game1.CAM.offset.X - 12),
                (mousePos.Y + Game1.CAM.offset.Y - 12)), position - Game1.CAM.offset);
        }
    }
}
