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



namespace RealTimeStrategy.Elements.Intelligence
{
    public class Node
    {
        public int F, G, H;
        public bool walkable;
        public Node parent;
        public Point position;
        public Node(int col, int row, bool w)
        {
            position = new Point(col, row);
            F = int.MaxValue;
            G = int.MaxValue;
            H = int.MaxValue;
            walkable = w;
        }
    }
}
