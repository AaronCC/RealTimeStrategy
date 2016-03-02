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
    public class Unit : GameObject
    {
        
        public Intelligence.Brain brain;
        public Rectangle oldHitBox;
        public int speed;
        public Vector2 velocity, direction;
        public Stack<Point> path;
        public double remaining;
        public bool arrived;
        public Unit(string _name, string _type, Vector2 _position)
            : base(_name, _type, _position)
        {
            arrived = false;
            velocity = new Vector2(0, 0);
            oldHitBox = hitBox;
            brain = new Intelligence.VillagerBrain(this);
        }
        public void GoTo(Point _target, Vector2 start)
        {
            arrived = false;
            target = _target;
            path = new Stack<Point>();
            Intelligence.PathGraph graph = new Intelligence.PathGraph();
            Intelligence.Node endNode = graph.NewPath(Game1.OBM.CalcChunkIndex(
                new Point((int)position.X,(int)position.Y)),
                Game1.OBM.CalcChunkIndex(new Point((int)target.X,(int)target.Y)));
            Intelligence.Node current = endNode;
            while(current.parent != null)
            {
                path.Push(current.position);
                current = current.parent;
            }
            Point chunkPos = Game1.OBM.CalcChunkIndex(new Point((int)position.X,(int)position.Y));
            remaining = Math.Abs(Math.Sqrt(Math.Pow((path.Peek().X * 25) - (chunkPos.X * 25), 2) + Math.Pow((path.Peek().Y * 25) - (chunkPos.Y * 25), 2)));
            Point _start = new Point((int)start.X, (int)start.Y);
            CalcVelocity(_target, _start);
        }
        public void CalcVelocity(Point _target, Point start)
        {
            velocity = Vector2.Zero;
            Point targetPoint;
            Point virt = new Point(path.Peek().X * Game1.OBM.GetWorld().chunkSize, path.Peek().Y * Game1.OBM.GetWorld().chunkSize);
            Point virtStart = new Point(start.X * Game1.OBM.GetWorld().chunkSize, start.Y * Game1.OBM.GetWorld().chunkSize) - new Point((int)Game1.CAM.offset.X, (int)Game1.CAM.offset.Y);
            targetPoint = (virt - new Point((int)Game1.CAM.offset.X, (int)Game1.CAM.offset.Y)) - virtStart;
            double targetVectorMagnitude = (Math.Sqrt(((Math.Pow(targetPoint.X, 2)) + (Math.Pow(targetPoint.Y, 2)))));
            direction.X = (float)(targetPoint.X * (1 / targetVectorMagnitude));
            direction.Y = (float)(targetPoint.Y * (1 / targetVectorMagnitude));
            velocity = speed * direction;
        }
        public virtual void Initialize(Point _target, Vector2 _start)
        {
            Game1.OBM.AddObject("Unit", this);
            GoTo(_target, _start);
        }
    }
}
