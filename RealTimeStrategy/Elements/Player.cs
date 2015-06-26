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
    public class Player
    {
        public KeyboardState kState;
        public MouseState mState;
        public KeyboardState old_kState;
        public MouseState old_mState;
        public Vector2 mousePos;
        public Rectangle mHitBox;
        public List<Objects.GameObject> selectedObjects;
        Assets.TextureAsset selectOverlay;
        Assets.TextureAsset selectBox;
        Assets.TextureAsset targetPoint;
        Rectangle selectRect;
        Vector2 startMousePos;
        bool selecting;
        bool singleSelect;
        Keys pressedKey;
        public Player()
        {
            selecting = false;
            singleSelect = false;
            selectBox = Game1.ASM.GetTextureAsset("SelectBox");
            selectOverlay = Game1.ASM.GetTextureAsset("SelectOverlay");
            targetPoint = Game1.ASM.GetTextureAsset("TargetPoint");
            kState = new KeyboardState();
            old_kState = new KeyboardState();
            mState = new MouseState();
            old_mState = new MouseState();
            selectedObjects = new List<Objects.GameObject>();
        }

        public void Update()
        {
            old_kState = kState;
            old_mState = mState;
            mState = Mouse.GetState();
            kState = Keyboard.GetState();
            mousePos.X = mState.Position.X / Game1.SCM.scalingFactor.X;
            mousePos.Y = mState.Position.Y / Game1.SCM.scalingFactor.Y;
            mHitBox = new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1);
            if (Game1.gameState == 1)
            {
                CheckSelect();
                if (selectedObjects.Count > 0 && mState.RightButton == ButtonState.Pressed && old_mState.RightButton == ButtonState.Released)
                    SelectAction();
                Keys[] pressedKeys = kState.GetPressedKeys();
                if (pressedKeys.Length > 0)
                {
                    if (pressedKey != pressedKeys[0])
                    {
                        pressedKey = pressedKeys[0];
                        ParseKeyInput(pressedKey);
                    }
                }
                else
                    pressedKey = Keys.None;
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle posRect;
            if (selecting)
            {
                spriteBatch.Draw(selectBox.sprite, selectRect, Color.White * 0.3f);
            }
            foreach (Objects.GameObject obj in selectedObjects)
            {
                spriteBatch.Draw(targetPoint.sprite,
                    new Vector2(obj.target.X - Game1.CAM.offset.X, obj.target.Y - Game1.CAM.offset.Y),
                    Color.White * 0.6f);
                if (obj.target != null)
                {
                    DrawLine(spriteBatch,
                        new Vector2(obj.position.X - Game1.CAM.offset.X + (obj.texture.sprite.Width / 2),
                                    obj.position.Y - Game1.CAM.offset.Y + (obj.texture.sprite.Height / 2)),
                        new Vector2(obj.target.X - Game1.CAM.offset.X + 12,
                                    obj.target.Y - Game1.CAM.offset.Y + 12));
                }
                posRect = new Rectangle(
                        obj.hitBox.X - (int)Game1.CAM.offset.X,
                        obj.hitBox.Y - (int)Game1.CAM.offset.Y,
                        obj.hitBox.Width,
                        obj.hitBox.Height);
                spriteBatch.Draw(selectOverlay.sprite, posRect, Color.White * 0.5f);
            }

        }
        void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end)
        {
            Vector2 edge = end - start;
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);
            sb.Draw(Game1.lineTexture,
                new Rectangle(
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(),
                    1),
                null,
                Color.Black * 0.6f,
                angle,
                new Vector2(0, 0),
                SpriteEffects.None,
                0);
        }
        private void CheckSelect()
        {
            if (mState.LeftButton == ButtonState.Pressed && old_mState.LeftButton == ButtonState.Pressed)
            {
                if (!selecting)
                {
                    selecting = true;
                    startMousePos = mousePos;
                    selectRect = new Rectangle((int)startMousePos.X, (int)startMousePos.Y, 0, 0);
                }
                selectRect.Width = (int)(mousePos.X - startMousePos.X);
                selectRect.Height = (int)(mousePos.Y - startMousePos.Y);
                if (selectRect.Width < 0 && selectRect.Height < 0)
                {
                    selectRect.X = (int)mousePos.X;
                    selectRect.Y = (int)mousePos.Y;
                }
                else if (selectRect.Width > 0 && selectRect.Height < 0)
                {
                    selectRect.X = (int)startMousePos.X;
                    selectRect.Y = (int)mousePos.Y;
                }
                else if (selectRect.Width < 0 && selectRect.Height > 0)
                {
                    selectRect.X = (int)mousePos.X;
                    selectRect.Y = (int)startMousePos.Y;
                }
                else
                {
                    selectRect.X = (int)startMousePos.X;
                    selectRect.Y = (int)startMousePos.Y;
                }
                selectRect.Width = Math.Abs(selectRect.Width);
                selectRect.Height = Math.Abs(selectRect.Height);
                if (selectRect.Width > 0 || selectRect.Height > 0)
                    singleSelect = false;
            }
            if (mState.LeftButton == ButtonState.Pressed && old_mState.LeftButton == ButtonState.Released)
            {
                selectRect = new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1);
                SelectObjects(selectRect);
                selecting = false;
                singleSelect = true;
            }
            if (selecting && mState.LeftButton == ButtonState.Released && !singleSelect)
            {
                SelectObjects(selectRect);
                selecting = false;
            }
        }
        public void SelectObjects(Rectangle selection)
        {
            foreach (Objects.GameObject obj in selectedObjects)
                obj.selected = false;
            selectedObjects = new List<Objects.GameObject>();
            selectedObjects =
                Game1.OBM.GetWorld().Query
                (Game1.OBM.CalcChunkIndex(new Point(selection.X + (int)Game1.CAM.offset.X,
                selection.Y + (int)Game1.CAM.offset.Y)),
                selection);
            foreach (Objects.GameObject obj in selectedObjects)
                obj.selected = true;
        }
        public void SelectAction()
        {
            foreach (Objects.GameObject obj in selectedObjects)
            {
                obj.RightClickAction(mousePos);
            }
        }
        public void ParseKeyInput(Keys key)
        {
            foreach (Objects.GameObject obj in selectedObjects)
            {
                obj.HotkeyAction(key);
            }
        }
    }
}
