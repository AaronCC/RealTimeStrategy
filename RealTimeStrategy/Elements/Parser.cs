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
    public class Parser
    {
        public Parser() { }
        public void Initialize() { }
        public Dictionary<string, string> ParseAssets(StreamReader file)
        {
            string line;
            Dictionary<string, string> assetDict = new Dictionary<string, string>();
            while ((line = file.ReadLine()) != null)
            {
                assetDict.Add(line.Split(':')[0], line.Split(':')[1]);
            }
            file.Close();
            return assetDict;
        }
        public Elements.Worlds.World ParseWorldData(StreamReader file)
        {
            Elements.Worlds.World world;
            int chunkR, chunkC, chunkS;
            string texture, name;
            name = file.ReadLine().Split(':')[1];
            chunkR = Convert.ToInt32(file.ReadLine().Split(':')[1]);
            chunkC = Convert.ToInt32(file.ReadLine().Split(':')[1]);
            chunkS = Convert.ToInt32(file.ReadLine().Split(':')[1]);
            texture = file.ReadLine().Split(':')[1];
            file.Close();
            switch (name)
            {
                case "GrassLands":
                    world = new Worlds.GrassLands(name, chunkR, chunkC, chunkS, texture);
                    break;
                default:
                    world = new Worlds.World(name, chunkR, chunkC, chunkS, texture);
                    break;
            }
            return world;
        }
        public Elements.Menus.GameMenu ParseMenuData(StreamReader file)
        {
            List<Elements.Buttons.Button> buttons = new List<Buttons.Button>();
            Elements.Menus.GameMenu menu;
            string name, line, bName;
            Vector2 position;
            string[] split;

            name = file.ReadLine().Split(':')[1];
            while ((line = file.ReadLine()) != null)
            {
                split = line.Split(':');
                bName = split[0];
                split = split[1].Split(',');
                position = new Vector2(Convert.ToInt32(split[0]), Convert.ToInt32(split[1]));
                switch (bName)
                {
                    case "PlayButton":
                        buttons.Add(new Buttons.PlayButton(position, bName));
                        break;
                    case "OptionsButton":
                        buttons.Add(new Buttons.OptionsButton(position, bName));
                        break;
                    case "BackButton":
                        buttons.Add(new Buttons.BackButton(position, bName));
                        break;
                    case "GrassLandsButton":
                        buttons.Add(new Buttons.GrassLandsButton(position, bName));
                        break;
                    default:
                        buttons.Add(new Buttons.Button(position, bName));
                        break;
                }
            }
            file.Close();
            switch (name)
            {
                case "MainMenu":
                    menu = new Elements.Menus.MainMenu(name, buttons);
                    break;
                case "OptionsMenu":
                    menu = new Elements.Menus.OptionsMenu(name, buttons);
                    break;
                case "WorldSelectMenu":
                    menu = new Elements.Menus.WorldSelectMenu(name, buttons);
                    break;
                default:
                    menu = new Elements.Menus.GameMenu(name, buttons);
                    break;
            }
            return menu;
        }
    }
}
