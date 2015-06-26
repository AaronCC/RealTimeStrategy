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

namespace RealTimeStrategy.Elements
{
    public class Camera
    {
        public Elements.Worlds.World world;
        public Rectangle visible;
        public Vector2 offset;
        Player player;
        public Camera(Vector2 resolution)
        {
            offset = new Vector2(0, 0);
            visible = new Rectangle(0, 0, (int)resolution.X, (int)resolution.Y);
        }
        public void Initialize(Elements.Worlds.World _world)
        {
            world = _world;
            offset.X = ((world.chunkC * world.chunkSize) / 2) - (visible.Width / 2);
            offset.Y = ((world.chunkR * world.chunkSize) / 2) - (visible.Height / 2);
            visible.X = (int)offset.X;
            visible.Y = (int)offset.Y;
        }
        public void Update()
        {
            player = Game1.OBM.GetPlayer();
            if (player.kState.IsKeyDown(Keys.Right) || player.mousePos.X > visible.Width - 10)
            {
                offset.X += 5;
            }
            if(player.kState.IsKeyDown(Keys.Down) || player.mousePos.Y > visible.Height - 10)
            {
                offset.Y += 5;
            }
            if (player.kState.IsKeyDown(Keys.Left) || player.mousePos.X < 10)
            {
                offset.X -= 5;
            }
            if (player.kState.IsKeyDown(Keys.Up) || player.mousePos.Y <  10)
            {
                offset.Y -= 5;
            }
            visible.X = (int)offset.X;
            visible.Y = (int)offset.Y;
        }
    }
}
