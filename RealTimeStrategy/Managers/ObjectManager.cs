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
namespace RealTimeStrategy.Managers
{
    public class ObjectManager
    {

        Stack<Elements.Menus.GameMenu> menuStack;
        Queue<Elements.Objects.GameObject> addObjectQueue;
        Queue<Elements.Objects.GameObject> removeObjectQueue;
        Dictionary<string, Elements.Objects.GameObject> objDict;
        public static Elements.Player PLAYER;
        public static Elements.Worlds.World WORLD;
        public static Elements.HUD.HeadsUpDisplay HUD;
        public static Stack<string> objectDrawStack;
        public static Stack<string> objectUpdateStack;
        int objCount = 0;
        public ObjectManager(string[] types)
        {
            objectDrawStack = new Stack<string>();
            objectUpdateStack = new Stack<string>();
            PLAYER = new Elements.Player();
            HUD = new Elements.HUD.HeadsUpDisplay();
            objDict = new Dictionary<string, Elements.Objects.GameObject>();
            addObjectQueue = new Queue<Elements.Objects.GameObject>();
            removeObjectQueue = new Queue<Elements.Objects.GameObject>();
            menuStack = new Stack<Elements.Menus.GameMenu>();
        }
        public Elements.Player GetPlayer() { return PLAYER; }
        public Elements.Worlds.World GetWorld() { return WORLD; }
        public void Initialize()
        {
            AddMenu(Game1.menuDict["MainMenu"]);
        }
        public void LoadWorld(string name)
        {
            WORLD = Game1.worldDict[name];
            WORLD.Generate();
            Game1.CAM.Initialize(WORLD);
        }
        public void PushDraw(string name)
        {
            if (!objectDrawStack.Contains(name))
                objectDrawStack.Push(name);
        }
        public void PushUpdate(string name)
        {
            if (!objectUpdateStack.Contains(name))
                objectUpdateStack.Push(name);
        }
        public void AddObject(string type, Elements.Objects.GameObject obj)
        {
            string c = "_";
            int dupIndex = 0;
            objCount++;
            while (true)
            {
                if (objDict.ContainsKey(obj.name) == false)
                {
                    objDict.Add(obj.name, obj);
                    WORLD._InsertObject(obj.hitBox, obj);
                    WORLD.UpdateObject(obj);
                    return;
                }
                else
                {
                    if (obj.name.Contains(c))
                    {
                        obj.name = obj.name.Substring(obj.name.IndexOf('_') + 1);
                    }
                    obj.name = dupIndex.ToString() + c + obj.name;
                    dupIndex++;
                }
            }
        }
        public Point CalcChunkIndex(Point index)
        {
            Point chunkIndex;
            chunkIndex = new Point((int)((index.X - (index.X % WORLD.chunkSize)) / WORLD.chunkSize),
                (int)((index.Y - (index.Y % WORLD.chunkSize)) / WORLD.chunkSize));
            return chunkIndex;
        }
        public void AddMenu(Elements.Menus.GameMenu menu)
        {
            menuStack.Push(menu);
        }

        public void RemoveObject(Elements.Objects.GameObject obj)
        {
            if (objDict.ContainsKey(obj.name))
            {
                Point startIndex;
                startIndex = new Point((int)((obj.position.X - (obj.position.X % WORLD.chunkSize)) / WORLD.chunkSize), (int)((obj.position.Y - (obj.position.Y % WORLD.chunkSize)) / WORLD.chunkSize));
                objDict.Remove(obj.name);
                WORLD._RemoveObject(obj.hitBox, obj);
            }
        }
        public void RemoveMenu()
        {
            if (menuStack.Peek() != null)
                menuStack.Pop();
        }
        public void UpdateMenus()
        {
            PLAYER.Update();
            if (menuStack.Count > 0)
                menuStack.Peek().Update();
        }
        public void UpdateGame()
        {
            PLAYER.Update();
            Game1.CAM.Update();
            WORLD.Update();
            while (objectUpdateStack.Count > 0)
            {
                objDict[objectUpdateStack.Pop()].Update();
            }
            HUD.Update();
        }
        public void DrawMenus(SpriteBatch spriteBatch)
        {
            if (menuStack.Count > 0)
                menuStack.Peek().Draw(spriteBatch);
        }
        public void DrawGame(SpriteBatch spriteBatch)
        {
            WORLD.Draw(spriteBatch);
            while (objectDrawStack.Count > 0)
            {
                objDict[objectDrawStack.Pop()].Draw(spriteBatch);
            }
            PLAYER.Draw(spriteBatch);
            HUD.Draw(spriteBatch);
            spriteBatch.DrawString(Game1.testFont, objCount.ToString(), new Vector2(5, 5), Color.White);
        }
    }
}
