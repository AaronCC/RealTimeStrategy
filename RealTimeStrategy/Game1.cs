#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
#endregion

namespace RealTimeStrategy
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>

    public class Game1 : Game
    {
        /// <summary>
        /// 0 : menu
        /// 1 : game
        /// 2 : paused
        /// </summary>
        public static int gameState;
        public string[] types;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static SpriteFont testFont;
        public static Elements.Parser PAR { get; set; }
        public static Elements.Camera CAM { get; set; }
        public static Managers.AssetManager ASM { get; set; }
        public static Managers.ObjectManager OBM { get; set; }
        public static Managers.ScreenManager SCM { get; set; }

        public static Dictionary<string, Elements.Worlds.World> worldDict;
        public static Dictionary<string, Elements.Menus.GameMenu> menuDict;
        public static Texture2D lineTexture;
        #region TestVars
        public Vector2 mousePos;
        #endregion
        Vector2 virtualResolution;


        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            worldDict = new Dictionary<string, Elements.Worlds.World>();
            menuDict = new Dictionary<string, Elements.Menus.GameMenu>();
            virtualResolution = new Vector2(1920, 1080);
            gameState = 0;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = true;
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            lineTexture = new Texture2D(GraphicsDevice, 1, 1);
            lineTexture.SetData<Color>(new Color[] { Color.Black * .6f });
            Texture2D tempTexture;
            SoundEffect tempSound;
            string data;
            testFont = Content.Load<SpriteFont>("Fonts/MyFont");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Foreach file in the "textures" directory, create a new element{type,emptyList} in objManager.objDict
            // Parse the "type".txt and create game textureAssets
            PAR = new Elements.Parser();
            ASM = new Managers.AssetManager();
            CAM = new Elements.Camera(virtualResolution);
            types = Directory.GetFiles("../../../Content/TextureData");
            foreach (string file in types)
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(file);
                foreach (KeyValuePair<string, string> csv in PAR.ParseAssets(reader))
                {
                    tempTexture = Content.Load<Texture2D>("Textures/" + csv.Value.Split('_')[0]);
                    data = csv.Value.Substring(csv.Value.IndexOf('_') + 1, (csv.Value.Length - csv.Value.IndexOf('_')) - 1);
                    ASM.CreateTextureAsset(csv.Key, data, tempTexture);
                }
            }
            OBM = new Managers.ObjectManager(types);
            types = Directory.GetFiles("../../../Content/SoundData");
            foreach(string file in types)
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(file);
                foreach(KeyValuePair<string,string> csv in PAR.ParseAssets(reader))
                {
                    tempSound = Content.Load<SoundEffect>("Sounds/" + csv.Value);
                    ASM.CreateSoundAsset(csv.Key, tempSound);
                }
            }
            types = Directory.GetFiles("../../../Content/WorldData");
            foreach(string file in types)
            {
                Elements.Worlds.World tempWorld;
                System.IO.StreamReader reader = new System.IO.StreamReader(file);
                tempWorld = PAR.ParseWorldData(reader);
                worldDict.Add(tempWorld.name, tempWorld);
            }
            types = Directory.GetFiles("../../../Content/MenuData");
            foreach (string file in types)
            {
                Elements.Menus.GameMenu tempMenu;
                System.IO.StreamReader reader = new System.IO.StreamReader(file);
                tempMenu = PAR.ParseMenuData(reader);
                menuDict.Add(tempMenu.name, tempMenu);
            }
            SCM = new Managers.ScreenManager(virtualResolution, new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
            SCM.CalcScaleFactor();
            OBM.Initialize();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            switch(gameState)
            {
                case 0:
                    OBM.UpdateMenus();
                    break;
                case 1:
                    OBM.UpdateGame(gameTime);
                    break;
                case 2:
                    break;
                default:
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.DarkSlateGray);
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, SCM.scale);
            switch (gameState)
            {
                case 0:
                    OBM.DrawMenus(spriteBatch);
                    break;
                case 1:
                    OBM.DrawGame(spriteBatch);
                    break;
                case 2:
                    OBM.DrawGame(spriteBatch);
                    break;
                default:
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
