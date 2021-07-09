using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE2.Data;
using SE2.Debug;
using SE2.Entities;
using SE2.Scenes;
using SE2.Utilities;

namespace SE2
{
    public class SE2 : Game
    {
        public GraphicsDeviceManager Graphics;
        private SpriteBatch _spriteBatch;

        public static bool Active { get; private set; }

        private Scene _activeScene;

        public static DebugText DebugText;

        public SE2()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            Window.AllowUserResizing = false;
            Window.ClientSizeChanged += OnSizeChanged;
        }

        protected override void Initialize()
        {
            base.Initialize();
            
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            XmlSerializer configSerializer = new XmlSerializer(typeof(GameConfig));
            
            if (File.Exists(Path.Combine(appData, "SE2-Data", "se2-config.cfg")))
            {
                using (Stream stream =
                    new FileStream(Path.Combine(appData, "SE2-Data", "se2-config.cfg"), FileMode.Open))
                {
                    GameConfig.LoadedConfig = (GameConfig) configSerializer.Deserialize(stream);
                    stream.Close();
                }
            }
            else
            {
                GameConfig.LoadedConfig = GameConfig.DefaultConfig;

                Directory.CreateDirectory(Path.Combine(appData, "SE2-Data"));
                using (Stream stream =
                    new FileStream(Path.Combine(appData, "SE2-Data", "se2-config.cfg"), FileMode.Create))
                {
                    configSerializer.Serialize(stream, GameConfig.LoadedConfig);
                    stream.Close();
                }
            }

            if (GameConfig.LoadedConfig.GetType().GetProperties().All(p => p.GetValue(GameConfig.LoadedConfig) == null))
                throw new ConfigException("Is your game config file out of date?");

            // Uncomment as necessary.
            SetScreenResolution(GameConfig.LoadedDisplayConfig.ScreenResolution, GameConfig.LoadedDisplayConfig.Fullscreen);
            //SetScreenResolution(1920, 1080);
            //SetScreenResolution(1920, 1080, true);

            if (GameDebug.EnableDebugMessages)
                DebugText = new DebugText(GraphicsDevice, new Vector2(0, Graphics.PreferredBackBufferHeight));
            
            // TODO: change this
            LoadScene(new IntroScene(this));
            //LoadScene(new MenuScene(this, true));
            //LoadScene(new MainScene(this));

            GameDebug.WriteLine("THIS BOX DISAPPEARS AFTER 5 SECONDS\nPRESS F12 TO MAKE IT REAPPEAR.");
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            FontManager.AddFont("Arial-12px", Content.Load<SpriteFont>("Fonts/Arial/Arial-12px"));
            FontManager.AddFont("Arial-256px", Content.Load<SpriteFont>("Fonts/Arial/Arial-256px"));
        }

        protected override void Update(GameTime gameTime)
        {
            Active = IsActive;

            _activeScene.Update(gameTime);
            DebugText?.Update(gameTime, Keyboard.GetState(), default);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            _activeScene.Draw(_spriteBatch);
            _spriteBatch.Begin();
            DebugText?.Draw(_spriteBatch);
            #if DEBUG
            _spriteBatch.DrawString(FontManager.GetFont("Arial-12px"), "PROTOTYPE - NOT FINAL", Vector2.Zero, Color.White);
            #endif
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void LoadScene(Scene scene)
        {
            GameDebug.WriteLine($"Loading scene of type: {scene.GetType()}");
            _activeScene?.UnloadContent();
            _activeScene = null;
            GameDebug.WriteLine("Current scene unloaded, collecting garbage...");
            GC.Collect();
            GameDebug.WriteLine("Scene's garbage has been collected.");

            _activeScene = scene;
            GameDebug.WriteLine("Initialising scene...");
            _activeScene.Initialize();
            GameDebug.WriteLine("Scene loaded.");
        }
        
        private void OnSizeChanged(object? sender, EventArgs e)
        {
            Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            if (DebugText != null)
                DebugText.Position.Y = Graphics.PreferredBackBufferHeight;
            //Graphics.ApplyChanges();
        }

        public void SetScreenResolution(int width, int height, bool fullScreen = false)
        {
            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.IsFullScreen = fullScreen;
            Graphics.ApplyChanges();
        }

        public void SetScreenResolution(Size size, bool fullScreen = false) =>
            SetScreenResolution((int) size.Width, (int) size.Height, fullScreen);
    }
}