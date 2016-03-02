#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace RealTimeStrategy.Elements.Worlds
{
    
    class GrassLands : World
    {
        int c = 0;
        public GrassLands(string n, int row, int col, int cS, string texture)
            : base(n, row, col, cS, texture)
        {
            chunkMap = new Chunk[row, col];
            name = n;
            chunkSize = cS;
            background = Game1.ASM.GetTextureAsset(texture);
            for (int c = 0; c < col; c++)
            {
                for (int r = 0; r < row; r++)
                {
                    chunkMap[r, c] = new Chunk("Grass", new Vector2(r * chunkSize, c * chunkSize), chunkSize);
                }
            }
        }
        public override void Update()
        {
            if (!themePlaying)
            {
                Game1.ASM.PlaySound("GrassLandsTheme", true);
                themePlaying = true;
            }
            foreach (Chunk chunk in chunkMap)
            {
                chunk.Update();
            }
            while (updateQueue.Count > 0)
            {
                updateQueue.Dequeue().PushUpdate();
            }
        }
        public override void Generate()
        {
            c++;
            Vector2 objPos;
            Rectangle visible = new Rectangle(((chunkC * chunkSize) / 2) - (1920 / 2), ((chunkR * chunkSize) / 2) - (1080 / 2), 1920, 1080);
            objPos = new Vector2((visible.X + (visible.Width / 2)) - 50,
               (visible.Y + (visible.Height / 2)) - 50);
            Game1.OBM.AddObject("Structure", new Objects.TownCenter("TownCenter", "Structure",objPos));
            noise = GeneratePerlinNoise(GenerateWhiteNoise(chunkC, chunkR), 6);
            for (int i = 0; i < chunkC; i++)
            {
                for (int j = 0; j < chunkR; j++)
                {
                    if (noise[i, j] > 0.75)
                    {
                        if (chunkMap[i, j].objects.Count == 0)
                            Game1.OBM.AddObject("Resource", new Elements.Objects.Tree("Tree", "Resource", new Vector2(i * chunkSize, j * chunkSize)));
                    }
                }
            }
        }
    }
}
