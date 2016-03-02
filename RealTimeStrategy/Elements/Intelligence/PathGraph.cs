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
    public class PathGraph
    {
        public List<Node> openList;
        public List<Node> closedList;
        Node[,] nodeMap;
        Node current;
        Point end;
        int reCount = 0;
        public PathGraph()
        {
            openList = new List<Node>();
            closedList = new List<Node>();
        }
        public void SetParent(int c, int r, Node parent)
        {
            nodeMap[c, r].parent = parent;
        }
        public Node NewPath(Point start, Point _end)
        {
            openList = new List<Node>();
            closedList = new List<Node>();
            nodeMap = new Node[Game1.OBM.GetWorld().chunkC, Game1.OBM.GetWorld().chunkR];
            for (int c = 0; c < Game1.OBM.GetWorld().chunkC; c++)
            {
                for (int r = 0; r < Game1.OBM.GetWorld().chunkR; r++)
                {
                    if (Game1.OBM.GetWorld().chunkMap[c, r].objects.Count > 0)
                        if (Game1.OBM.GetWorld().chunkMap[c, r].objects[0] as Objects.TownCenter != null)
                            nodeMap[c, r] = new Node(c, r, true);
                        else
                            nodeMap[c, r] = new Node(c, r, false);
                    else
                        nodeMap[c, r] = new Node(c, r, true);
                }
            }
            current = nodeMap[start.X, start.Y];
            end = _end;
            current.G = 0;
            current.H = Heuristic(current, end);
            current.F = int.MaxValue;
            openList.Add(current);
            FindPath();
            return current;
        }
        public void FindPath()
        {
            if (reCount > 100)
            {

            }
            else if (current.position != end)
            {
                int min = int.MaxValue;
                foreach (Node node in openList)
                {
                    if (node.F < min)
                    {
                        current = node;
                        min = current.F;
                    }
                }
                openList.Remove(current);
                closedList.Add(current);
                Point start = current.position;
                foreach (Node adj in Neighbors(start))
                {
                    if(!openList.Contains(adj))
                    {
                        openList.Add(adj);
                        adj.parent = current;
                        adj.G = Distance(current, adj);
                        adj.H = Heuristic(adj, end);
                        adj.F = adj.G + adj.H;
                    }
                    else if(openList.Contains(adj))
                    {
                        if(Distance(current,adj) < adj.G)
                        {
                            adj.G = Distance(current, adj);
                            adj.F = adj.G + adj.H;
                            adj.parent = current;
                        }
                    }
                }
                reCount++;
                FindPath();
            }
        }
        public int Distance(Node node, Node adj)
        {
            if (node.position.X == adj.position.X || node.position.Y == adj.position.Y)
                return 10;
            else
                return 14;
        }
        public int Heuristic(Node adj, Point end)
        {
            int hSquares = 0;
            if (adj.position.X < end.X)
                for (int x = adj.position.X; x < end.X; x++)
                    hSquares++;
            if (adj.position.X > end.X)
                for (int x = adj.position.X; x > end.X; x--)
                    hSquares++;
            if (adj.position.Y < end.Y)
                for (int x = adj.position.Y; x < end.Y; x++)
                    hSquares++;
            if (adj.position.Y > end.Y)
                for (int x = adj.position.Y; x > end.Y; x--)
                    hSquares++;
            return 10 * hSquares;
        }
        public List<Node> Neighbors(Point start)
        {
            List<Node> neighbors = new List<Node>();
            for (int c = start.X - 1; c <= start.X + 1; c++)
            {
                for (int r = start.Y - 1; r <= start.Y + 1; r++)
                {
                    if (!(c == start.X && r == start.Y) && nodeMap[c, r].walkable && !closedList.Contains(nodeMap[c,r]))
                    {
                        neighbors.Add(nodeMap[c, r]);
                    }
                }
            }
            return neighbors;
        }
    }
}
