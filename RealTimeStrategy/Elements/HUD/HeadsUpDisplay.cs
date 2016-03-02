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
        Assets.TextureAsset loadBox;
        Assets.TextureAsset loadBar;
        Rectangle overlayRect;
        Rectangle stretchRect;
        Rectangle selectRect;
        Rectangle loadRect;
        Rectangle loadBarRect;
        public List<Rectangle> loadQueueRects;
        public Rectangle hotkeyRect;
        Objects.GameObject selectedObject;
        List<Objects.GameObject> selectedObjects;
        int stretchSize;
        int stretchBuffer;
        int selectedCapacity;
        int selectedCount;
        public HeadsUpDisplay()
        {
            loadQueueRects = new List<Rectangle>();
            selectedObjects = new List<Objects.GameObject>();
            stretchBuffer = 10;
            stretchSize = 50;
            overlay = Game1.ASM.GetTextureAsset("HUDOverlay");
            loadBox = Game1.ASM.GetTextureAsset("LoadBox");
            loadBar = Game1.ASM.GetTextureAsset("LoadBar");
            overlayRect = new Rectangle(0, 900, 1920, 180);
            selectRect = new Rectangle(0, 900, 630, 180);
            loadRect = new Rectangle(730, 910, 300, 150);
            loadBarRect = new Rectangle(830, 940, 0, 12);
            hotkeyRect = new Rectangle(1130, 910, 300, 300);
            loadQueueRects.Add(new Rectangle(730, 910, 75, 75));
            loadQueueRects.Add(new Rectangle(730, 985, 75, 75));
            loadQueueRects.Add(new Rectangle(805, 985, 75, 75));
            loadQueueRects.Add(new Rectangle(880, 985, 75, 75));
            loadQueueRects.Add(new Rectangle(955, 985, 75, 75));
            selectedCapacity = ((selectRect.Width - (2 * stretchBuffer)) / stretchSize) * ((selectRect.Height - (2 * stretchBuffer)) / stretchSize);
            selectedCount = 0;
        }
        public void Update()
        {
            stretchRect = new Rectangle(selectRect.X + stretchBuffer, selectRect.Y + stretchBuffer, stretchSize, stretchSize);
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(overlay.sprite, Vector2.Zero, Color.White * 0.5f);
            selectedObjects = Game1.OBM.GetPlayer().selectedObjects;

            foreach (Objects.GameObject obj in selectedObjects)
            {
                if (selectedCount < selectedCapacity)
                {
                    if (stretchRect.X + stretchSize < selectRect.X + selectRect.Width)
                    {
                        spriteBatch.Draw(obj.texture.sprite, stretchRect, Color.White);
                        selectedCount++;
                        stretchRect.X += stretchSize + 1;
                    }
                    else
                    {
                        stretchRect.X = selectRect.X + stretchBuffer;
                        stretchRect.Y += stretchSize + 1;
                    }
                }
            }
            if (selectedObjects.Count > 0)
            {
                int i = 0;
                while (selectedObjects[i] as Objects.Structure == null)
                {
                    if (i == selectedObjects.Count - 1)
                        break;
                    i++;
                }
                selectedObject = selectedObjects[i];
                for (int x = 0; x < selectedObject.hotKeys.Count; x++)
                {
                    Rectangle drawRect = new Rectangle(1130 + (75 * x), 910 + (75 * x), 75, 75);
                    spriteBatch.Draw(selectedObject.hotKeyImages[x].Value.sprite, drawRect, Color.White);
                    spriteBatch.DrawString(Game1.testFont, selectedObject.hotKeyImages[x].Key.ToString(), new Vector2(drawRect.X, drawRect.Y), Color.White);
                }
                if (selectedObject as Objects.Structure != null)
                {
                    Objects.Structure structure = (Objects.Structure)selectedObject;
                    if (structure.loadQueue.Count > 0)
                    {
                        double percent = 1 - (double)structure.currentLoadTime / structure.loadQueue.Peek().Key;
                        loadBarRect.Width = (int)(170 * percent);
                        spriteBatch.Draw(loadBar.sprite, loadBarRect, Color.White);
                        spriteBatch.DrawString(Game1.testFont, ((structure.currentLoadTime + 999) / 1000).ToString(), new Vector2(830, loadBarRect.Y - 25), Color.Black);
                        for (int x = 0; x < structure.loadQueue.Count; x++)
                        {
                            spriteBatch.Draw(structure.loadQueue.Peek().Value.texture.sprite, loadQueueRects[x], Color.White);
                        }
                    }
                    spriteBatch.Draw(loadBox.sprite, loadRect, Color.White);
                }
            }
            selectedCount = 0;
        }
    }
}
