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

namespace RealTimeStrategy.Elements.Menus
{
    public class MainMenu : GameMenu
    {

        public MainMenu(string n, List<Buttons.Button> butts)
            : base(n, butts)
        {
            themePlaying = false;    
            buttons = butts;
            name = n;
            texture = Game1.ASM.GetTextureAsset(name);
        }
       
    }
}
