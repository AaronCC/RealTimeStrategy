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
    public class World
    {
        public Queue<Objects.GameObject> updateQueue;
        public Chunk[,] chunkMap;
        public string name;
        public int chunkSize, chunkR, chunkC;
        public Assets.TextureAsset background;
        public bool themePlaying;
        public double[,] noise;
        Random random;
        public World(string n, int row, int col, int cS, string texture)
        {
            updateQueue = new Queue<Objects.GameObject>();
            random = new Random(0);
            themePlaying = false;
            chunkR = row;
            chunkC = col;
            chunkMap = new Chunk[row, col];
            name = n;
            chunkSize = cS;
            background = Game1.ASM.GetTextureAsset(texture);
        }
        public virtual void UpdateObject(Objects.GameObject obj)
        {
            updateQueue.Enqueue(obj);
        }
        public void LoadChunk(Point index, Chunk chunk)
        {
            chunkMap[index.Y, index.X] = chunk;
        }
        public virtual void Update()
        {
            Rectangle visible = Game1.CAM.visible;
            Point start = new Point((visible.Y - (visible.Y % chunkSize)) / chunkSize, (visible.X - (visible.X % chunkSize)) / chunkSize);
            Point end = new Point(((visible.Y + visible.Height) - ((visible.Y + visible.Height) % chunkSize)) / chunkSize,
                ((visible.X + visible.Width) - ((visible.X + visible.Width) % chunkSize)) / chunkSize);

            for (int col = start.X; col <= end.X; col++)
            {
                for (int row = start.Y; row <= end.Y; row++)
                {
                    if ((row < chunkR && col < chunkC) && (row >= 0 && col >= 0))
                        chunkMap[row, col].Update();
                }
            }
        }
        public void InsertObject(Rectangle hitBox, Objects.GameObject obj)
        {
            Point start, end;
            start = Game1.OBM.CalcChunkIndex(new Point((int)hitBox.X, (int)hitBox.Y));
            end = Game1.OBM.CalcChunkIndex(new Point((int)(hitBox.X + hitBox.Width), (int)(hitBox.Y + hitBox.Height)));
            for (int x = start.X; x <= end.X; x++)
            {
                for (int y = start.Y; y <= end.Y; y++)
                {
                    if (x < 200 && y < 200)
                        chunkMap[x, y].objects.Add(obj);
                }
            }
        }
        public void RemoveObject(Rectangle hitBox, Objects.GameObject obj)
        {
            Point start, end;
            start = Game1.OBM.CalcChunkIndex(new Point((int)hitBox.X, (int)hitBox.Y));
            end = Game1.OBM.CalcChunkIndex(new Point((int)(hitBox.X + hitBox.Width), (int)(hitBox.Y + hitBox.Height)));
            for (int x = start.X; x <= end.X; x++)
            {
                for (int y = start.Y; y <= end.Y; y++)
                {
                    if (x < 200 && y < 200)
                        chunkMap[x, y].objects.Remove(obj);
                }
            }
        }
        public void _InsertObject(Rectangle hitBox, Objects.GameObject obj)
        {
            Point start, end;
            start = Game1.OBM.CalcChunkIndex(new Point((int)hitBox.X, (int)hitBox.Y));
            end = Game1.OBM.CalcChunkIndex(new Point((int)(hitBox.X + hitBox.Width), (int)(hitBox.Y + hitBox.Height)));
            for (int x = start.X; x < end.X; x++)
            {
                for (int y = start.Y; y < end.Y; y++)
                {
                    if (x < 200 && y < 200)
                        chunkMap[x, y].objects.Add(obj);
                }
            }
        }
        public void _RemoveObject(Rectangle hitBox, Objects.GameObject obj)
        {
            Point start, end;
            start = Game1.OBM.CalcChunkIndex(new Point((int)hitBox.X, (int)hitBox.Y));
            end = Game1.OBM.CalcChunkIndex(new Point((int)(hitBox.X + hitBox.Width), (int)(hitBox.Y + hitBox.Height)));
            for (int x = start.X; x < end.X; x++)
            {
                for (int y = start.Y; y < end.Y; y++)
                {
                    if (x < 200 && y < 200)
                        chunkMap[x, y].objects.Remove(obj);
                }
            }
        }

        public List<Objects.GameObject> Query(Point index, Rectangle hitBox)
        {
            List<Objects.GameObject> queryObjects;
            queryObjects = new List<Objects.GameObject>();
            Point endIndex = index;
            endIndex.X += hitBox.Width / 25;
            endIndex.Y += hitBox.Height / 25;
            for (int x = index.X; x <= endIndex.X; x++)
            {
                for (int y = index.Y; y <= endIndex.Y; y++)
                {
                    
                    foreach (Objects.GameObject obj in chunkMap[x, y].objects)
                    {
                        if (!queryObjects.Contains(obj) && hitBox.Intersects
                            (new Rectangle((int)(obj.hitBox.X - Game1.CAM.offset.X), (int)(obj.hitBox.Y - Game1.CAM.offset.Y),obj.hitBox.Width,
                                obj.hitBox.Height)))
                            queryObjects.Add(obj);
                    }
                }
            }
            return queryObjects;
        }
        public virtual void Generate()
        {
            noise = GeneratePerlinNoise(GenerateWhiteNoise(200, 200), 4);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle visible = Game1.CAM.visible;
            Point start = new Point((visible.Y - (visible.Y % chunkSize)) / chunkSize, (visible.X - (visible.X % chunkSize)) / chunkSize);
            Point end = new Point(((visible.Y + visible.Height) - ((visible.Y + visible.Height) % chunkSize)) / chunkSize,
                ((visible.X + visible.Width) - ((visible.X + visible.Width) % chunkSize)) / chunkSize);

            for (int col = start.X; col <= end.X; col++)
            {
                for (int row = start.Y; row <= end.Y; row++)
                {
                    if ((row < chunkR && col < chunkC) && (row >= 0 && col >= 0))
                        chunkMap[row, col].Draw(spriteBatch);

                }
            }
        }
        #region PerlinNoise
        public double[,] GeneratePerlinNoise(double[,] baseNoise, int octaveCount)
        {
            int width = baseNoise.GetLength(0);
            int height = baseNoise.GetLength(1);
            
            double[][,] smoothNoise = new double[octaveCount][,];

            double persistance = 0.5;

            for (int i = 0; i < octaveCount; i++)
            {
                smoothNoise[i] = GenerateSmoothNoise(baseNoise, i);
            }

            double[,] perlinNoise = new double[width, height];
            double amplitude = 1.0;
            double totalAmplitude = 0.0;

            for (int octave = octaveCount - 1; octave >= 0; octave--)
            {
                amplitude *= persistance;
                totalAmplitude += amplitude;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        perlinNoise[i, j] += smoothNoise[octave][i, j] * amplitude;
                    }
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    perlinNoise[i, j] /= totalAmplitude;
                }
            }
            return perlinNoise;
        }
        public double[,] GenerateWhiteNoise(int width, int height)
        {
            random = new Random(Guid.NewGuid().GetHashCode());
            double[,] whiteNoise = new double[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    whiteNoise[i, j] = random.NextDouble() % 1;
                }
            }
            return whiteNoise;
        }
        public double[,] GenerateSmoothNoise(double[,] baseNoise, int octave)
        {
            int width = baseNoise.GetLength(0);
            int height = baseNoise.GetLength(1);
            double[,] smoothNoise = new double[width, height];
            int samplePeriod = 1 << octave;
            double sampleFrequency = 1.0 / samplePeriod;

            for (int i = 0; i < width; i++)
            {
                int i0 = (i / samplePeriod) * samplePeriod;
                int i1 = (i0 + samplePeriod) % width;
                double horizontal_blend = (i - i0) * sampleFrequency;

                for (int j = 0; j < height; j++)
                {
                    int j0 = (j / samplePeriod) * samplePeriod;
                    int j1 = (j0 + samplePeriod) % height;
                    double vertical_blend = (j - j0) * sampleFrequency;

                    double top = Interpolate(baseNoise[i0, j0], baseNoise[i1, j0], horizontal_blend);
                    double bottom = Interpolate(baseNoise[i0, j1], baseNoise[i1, j1], horizontal_blend);

                    smoothNoise[i, j] = Interpolate(top, bottom, vertical_blend);
                }
            }

            return smoothNoise;
        }

        public double Interpolate(double x0, double x1, double alpha)
        {
            return x0 * (1 - alpha) + alpha * x1;
        }
        #endregion
    }
}
