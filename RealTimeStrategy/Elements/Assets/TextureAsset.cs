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
namespace RealTimeStrategy.Elements.Assets
{
    public class TextureAsset
    {
        public Texture2D sprite { get; set; }
        public int rows { get; set; }
        public int cols { get; set; }
        public int mpf { get; set; }
        public string name;
        public TextureAsset(string n, Texture2D s, int r, int c, int m)
        {
            name = n;
            sprite = s;
            rows = r;
            cols = c;
            mpf = m;
        }
    }
}
