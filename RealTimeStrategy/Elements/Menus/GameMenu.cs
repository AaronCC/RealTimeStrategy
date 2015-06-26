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
    public class GameMenu
    {
        public List<Buttons.Button> buttons;
        public Assets.TextureAsset texture;
        public string name;
        public bool themePlaying;
        public GameMenu(string n, List<Buttons.Button> butts)
        {
            themePlaying = false;
            buttons = butts;
            name = n;
            texture = Game1.ASM.GetTextureAsset(name);
        }
        public void Update()
        {
            if (!themePlaying)
            {
                Game1.ASM.PlaySound("MenuTheme", true);
            }
            foreach (Buttons.Button button in buttons)
            {
                if (Game1.OBM.GetPlayer().mHitBox.Intersects(button.hitBox) && button.flagged == false)
                {
                    Game1.ASM.PlaySound("Click", false);
                    button.flagged = true;
                }

                else if (!Game1.OBM.GetPlayer().mHitBox.Intersects(button.hitBox))
                    button.flagged = false;

                if (Game1.OBM.GetPlayer().mState.LeftButton == ButtonState.Pressed && Game1.OBM.GetPlayer().old_mState.LeftButton == ButtonState.Released && button.flagged == true)
                {
                    button.ClickEvent();
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture.sprite, new Vector2(0, 0), Color.White);
            foreach (Buttons.Button button in buttons)
            {
                if (button.flagged)
                {
                    spriteBatch.Draw(button.texture.sprite, button.position, Color.White);
                }
                else
                {
                    spriteBatch.Draw(button.texture.sprite, button.position, Color.White * 0.7f);
                }
            }
        }
    }
}
