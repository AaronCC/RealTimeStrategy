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

namespace RealTimeStrategy.Elements.HUD
{
    public class HeadsUpDisplay
    {
        Assets.TextureAsset overlay;
        Rectangle overlayBox;
        Rectangle stretchBox;
        Rectangle selectBox;

        int stretchSize;
        int stretchBuffer;
        int selectedCapacity;
        int selectedCount;
        public HeadsUpDisplay()
        {
            stretchBuffer = 10;
            stretchSize = 50;
            overlay = Game1.ASM.GetTextureAsset("HUDOverlay");
            overlayBox = new Rectangle(0, 900, 1920, 180);
            selectBox = new Rectangle(0, 900, 630, 180);
            selectedCapacity = ((selectBox.Width - (2 * stretchBuffer)) / stretchSize) * ((selectBox.Height - (2 * stretchBuffer)) / stretchSize);
            selectedCount = 0;
        }
        public void Update()
        {
            stretchBox = new Rectangle(selectBox.X + stretchBuffer, selectBox.Y + stretchBuffer, stretchSize, stretchSize);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(overlay.sprite, Vector2.Zero, Color.White * 0.5f);
            foreach(Objects.GameObject obj in Game1.OBM.GetPlayer().selectedObjects)
            {
                if (selectedCount < selectedCapacity)
                {
                    if (stretchBox.X + stretchSize < selectBox.X + selectBox.Width)
                    {
                        spriteBatch.Draw(obj.texture.sprite, stretchBox, Color.White);
                        selectedCount++;
                        stretchBox.X += stretchSize + 1;
                    }
                    else
                    {
                        stretchBox.X = selectBox.X + stretchBuffer;
                        stretchBox.Y += stretchSize + 1;
                    }
                }
            }
            selectedCount = 0;
        }
    }
}
