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
    public class Chunk
    {
        public List<Elements.Objects.GameObject> objects;
        Vector2 position;
        public Rectangle drawRect;
        Elements.Assets.TextureAsset texture;
        Elements.Assets.TextureAsset occTexture;
        public int size;
        int counted = 1;
        public Chunk(string textureName, Vector2 pos, int _size)
        {
            size = _size;
            position = pos;
            objects = new List<Elements.Objects.GameObject>();
            texture = Game1.ASM.GetTextureAsset(textureName);
            occTexture = Game1.ASM.GetTextureAsset("RedChunk");
            drawRect = new Rectangle((int)(position.X - Game1.CAM.offset.X), (int)(position.Y - Game1.CAM.offset.Y), size, size);
        }

        public void Update()
        {
            drawRect.X = (int)(position.X - Game1.CAM.offset.X);
            drawRect.Y = (int)(position.Y - Game1.CAM.offset.Y);
            //foreach (Elements.Objects.GameObject obj in objects)
            //{
            //    Game1.OBM.PushUpdate(obj.name);
            //}
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //if (objects.Count > 0)
            //    spriteBatch.Draw(occTexture.sprite, drawRect, Color.White);
            //else
                spriteBatch.Draw(texture.sprite, drawRect, Color.White);
            
            foreach (Elements.Objects.GameObject obj in objects)
            {
                Game1.OBM.PushDraw(obj.name);
            }
            //spriteBatch.DrawString(Game1.testFont, objects.Count.ToString(), new Vector2(drawRect.X, drawRect.Y), Color.White);
        }
    }
}
