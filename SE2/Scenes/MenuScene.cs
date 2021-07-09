using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE2.Data;
using SE2.Debug;
using SE2.UI;
using SE2.UI.Panels;
using SE2.Utilities;

namespace SE2.Scenes
{
    // The game's main menu.
    public class MenuScene : Scene
    {
        private bool _transition; // If true, the game will transition from one slide to the next.

        private List<MenuImage> _images; // A list of menu images to be displayed in the background.

        private MenuImage _activeMenuImage; // The active menu image currently being displayed.
        // The menu image that will be transitioned to when it is time. This will only display during the transition
        // period, and will then become the active menu image.
        private MenuImage _transitionMenuImage;
        private int _nextImage; // The next image that will be transitioned to.

        // The number of total game seconds that have passed when the transition was called.
        private float _startingSeconds;

        private Matrix _transformMatrix;

        private bool _fromIntro; // If false, the game will pick a random image to 

        private bool _escapeHeld;

        private RasterizerState _rasterizerState;

        public MenuScene(SE2 se2, bool fromIntro = false) : base(se2)
        {
            _fromIntro = fromIntro;
        }

        public override void Initialize()
        {
            base.Initialize();

            UIManager.Clear();
            
            GameDebug.WriteLine("Main menu initialising...");

            Color buttonColor = Color.GhostWhite;
            Color hoverColor = Color.DarkGray;
            
            UIButton continueButton = new UIButton(GraphicsDevice, new Vector2(50, 200), new Size(250, 100), buttonColor,
                "Continue Game", 5, Color.White, Color.Black) { HoverColor = hoverColor };
            continueButton.OnClick += (sender, args) =>
            {
                UIManager.Clear();   
                SE2.LoadScene(new MainScene(SE2));
            };
            //continueButton.Show = false;

            UIButton newGameButton = new UIButton(GraphicsDevice, continueButton.Show ? new Vector2(continueButton.Position.X, continueButton.Position.Y + 120) : continueButton.Position, !continueButton.Show ? continueButton.Size : new Size(250, 50), buttonColor, "New Game", 5, Color.White, Color.Black) {HoverColor = hoverColor};
            newGameButton.OnClick += (sender, args) => OpenPanel("newGamePanel");
            
            UIButton loadGameButton = new UIButton(GraphicsDevice,
                new Vector2(newGameButton.Position.X, newGameButton.Position.Y + newGameButton.Size.Height + 20), new Size(250, 50), buttonColor,
                "Load Game", 5, Color.White, Color.Black) {HoverColor = hoverColor};
            
            UIButton joinGameButton = new UIButton(GraphicsDevice,
                new Vector2(loadGameButton.Position.X, loadGameButton.Position.Y + 70), new Size(250, 50), buttonColor,
                "Multiplayer", 5, Color.White, Color.Black) {HoverColor = hoverColor};
            
            UIButton settingsGameButton = new UIButton(GraphicsDevice,
                new Vector2(joinGameButton.Position.X, joinGameButton.Position.Y + 70), new Size(250, 50), buttonColor,
                "Options", 5, Color.White, Color.Black) {HoverColor = hoverColor};
            settingsGameButton.OnClick += (sender, args) => OpenPanel("settingsPanel");
            
            UIButton quitButton =
                new UIButton(GraphicsDevice, new Vector2(settingsGameButton.Position.X, settingsGameButton.Position.Y + 70), new Size(250, 50), Color.GhostWhite, "Quit", 5,
                    Color.White, Color.Black) { HoverColor = Color.DarkGray };
            quitButton.OnClick += (sender, args) => SE2.Exit();
            
            UIText text = new UIText(new Vector2(175, 100), "Arial", "SE2", 100)
                {Alignment = TextAlignment.CenterMiddle};

            UIPanel backgroundPanel = new UIPanel(GraphicsDevice, Vector2.Zero,
                new Size(SE2.Graphics.PreferredBackBufferWidth, SE2.Graphics.PreferredBackBufferHeight),
                Color.Black * 0.5f) { Show = false };
            UIPanel settingsPanel = new UIPanel(GraphicsDevice, new Vector2(390, 110), new Size(500, 500), buttonColor,
                5, Color.White) { Show = false };

            UIPanel newGamePanel = new UIPanel(GraphicsDevice, new Vector2(100, 110), new Size(1080, 500), buttonColor,
                5, Color.White) {Show = false};
            
            UIManager.Add("continueButton", continueButton);
            UIManager.Add("newGameButton", newGameButton);
            UIManager.Add("loadGameButton", loadGameButton);
            UIManager.Add("joinGameButton", joinGameButton);
            UIManager.Add("settingsGameButton", settingsGameButton);
            UIManager.Add("quitButton", quitButton);
            UIManager.Add("text", text);
            UIManager.Add("backgroundPanel", backgroundPanel);
            UIManager.Add("settingsPanel", settingsPanel);
            UIManager.Add("newGamePanel", newGamePanel);
            
            #region Settings Panel

            UIButton settingsDisplayTab = new UIButton(GraphicsDevice, settingsPanel.Position + new Vector2(10),
                new Size(100, 30), Color.DarkGray, "Display", 3, Color.White, Color.White, 18)
                { Parent = settingsPanel };
            
            UIButton settingsControlsTab = new UIButton(GraphicsDevice, settingsDisplayTab.Position + new Vector2(settingsDisplayTab.Size.Width + 10, 0),
                    new Size(100, 30), Color.DarkGray, "Controls", 3, Color.White, Color.White, 18)
                { Parent = settingsPanel };

            UIText settingsFullScreenText = new UIText(new Vector2(400, 200), "Arial", "Fullscreen")
                {Parent = settingsPanel, Color = Color.Black};

            UICheckBox settingsFullScreenCheckbox = new UICheckBox(GraphicsDevice, new Vector2(560, 200),
                    new Size(40, 40), Color.DarkGray, Color.White, 3, Color.White, SE2.Graphics.IsFullScreen)
                {Parent = settingsPanel};
            settingsFullScreenCheckbox.OnCheck += (sender, args) =>
                SE2.SetScreenResolution(GameConfig.LoadedDisplayConfig.ScreenResolution, args.Checked);

            DisplayModeCollection modes = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes;
            DisplayMode currentMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            
            UiDropDown settingsResolutionDropDown = new UiDropDown(GraphicsDevice, new Vector2(400, 150), new Size(200, 20),
                Color.DarkGray, $"{SE2.Graphics.PreferredBackBufferWidth}x{SE2.Graphics.PreferredBackBufferHeight}", fontSize: 12)
                { Parent = settingsPanel };
            foreach (DisplayMode mode in modes)
                settingsResolutionDropDown.Add($"{mode.Width}x{mode.Height}");
            settingsResolutionDropDown.OnSelectionChanged += (sender, args) =>
                GameDebug.WriteLine($"Selection changed to: {settingsResolutionDropDown.SelectedItem.Text}");


            UICloseButton settingsCloseButton =
                new UICloseButton(GraphicsDevice, new Vector2(860, 120), new Size(20, 20), Color.Black)
                    {Parent = settingsPanel};
            settingsCloseButton.OnClick += (sender, args) => ClosePanel("settingsPanel");
            
            UIManager.Add("settingsDisplayTab", settingsDisplayTab);
            UIManager.Add("settingsControlsTab", settingsControlsTab);
            UIManager.Add("settingsFullScreenText", settingsFullScreenText);
            UIManager.Add("settingsFullScreenCheckbox", settingsFullScreenCheckbox);
            UIManager.Add("settingsResolutionDropDown", settingsResolutionDropDown);
            UIManager.Add("settingsCloseButton", settingsCloseButton);
            
            #endregion

            #region New Game Panel
            
            

            #endregion

            SE2.IsMouseVisible = true;

            _rasterizerState = new RasterizerState {ScissorTestEnable = true};

            // Annoyingly, we need to set this each time there is a transition. Oh well.
            _activeMenuImage.MaxScaleReached += (sender, args) =>
            {
                _transition = true;
            };
        }
        
        protected override void LoadContent()
        {
            // Load some backgrounds in.
            GameDebug.WriteLine("Loading backgrounds...");
            _images = new List<MenuImage>();
            Texture2D image = Content.Load<Texture2D>("Images/se2 logo large blurred");
            _images.Add(new MenuImage(image,
                new Vector2(image.Width, image.Height) / 2 - new Vector2(1280, 720) / 2 +
                new Vector2(500, 175)) { InitialScale = 0.9f });
            image = Content.Load<Texture2D>("Images/PLACEHOLDER background 1");
            _images.Add(new MenuImage(image, new Vector2(1280, 720) / 2));
            image = Content.Load<Texture2D>("Images/PLACEHOLDER background 2");
            _images.Add(new MenuImage(image, new Vector2(1280, 720) / 2));
            image = Content.Load<Texture2D>("Images/PLACEHOLDER background 3");
            _images.Add(new MenuImage(image, new Vector2(1280, 720) / 2));
            GameDebug.WriteLine("Background loading done!");

            // Set the current active menu image, as well as the transition image.
            if (!_fromIntro)
            {
                Random random = new Random();
                int randImage = random.Next(_images.Count - 1);
                _activeMenuImage = _images[randImage];
                _nextImage = randImage + 1 >= _images.Count ? 0 : randImage + 1;
            }
            else
            {
                _activeMenuImage = _images[0];
                // The next image is image 1.
                _nextImage = 1;
            }
            
            _transitionMenuImage = _images[_nextImage];

            // A menu image's alpha is 0 by default, so we need to set it to 1.
            _activeMenuImage.Alpha = 1;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mState = Mouse.GetState();
            KeyboardState kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.Escape))
            {
                if (!_escapeHeld)
                {
                    _escapeHeld = true;
                    
                    if (UIManager.GetElement("settingsPanel").Show)
                        ClosePanel("settingsPanel");
                    else if (UIManager.GetElement("newGamePanel").Show)
                        ClosePanel("newGamePanel");
                    else
                        SE2.Exit();
                }
            }
            else 
                _escapeHeld = false;

            Vector2 mousePos = Vector2.Transform(new Vector2(mState.X, mState.Y), Matrix.Invert(_transformMatrix));
            
            //GameDebug.WriteLine(mousePos);
            
            //_playButton.Update(gameTime, default, mState, mousePos);
            //_quitButton.Update(gameTime, default, mState, mousePos);
            
            UIManager.Update(gameTime, kState, mState, mousePos);
            
            _activeMenuImage.Update(gameTime);
            
            if (_transition)
            {
                // Checks to see if the starting seconds have been set.
                // If not, it sets them.
                if (_startingSeconds == default)
                    _startingSeconds = (float) gameTime.TotalGameTime.TotalSeconds;

                // Runs once the transition image's alpha is 1 (completely visible)
                if (_transitionMenuImage.Alpha >= 1)
                {
                    _transition = false; // Disable transition, as the transition is now over.
                    _transitionMenuImage.Alpha = 1; // Force the alpha of the image to be 1 (to compensate if the alpha is above 1)
                    _activeMenuImage.Reset(); // Reset the active image's parameters, so it can be used again.
                    _activeMenuImage = _transitionMenuImage; // Set the active image to be the transition image.
                    _nextImage = _nextImage + 1 >= _images.Count ? 0 : _nextImage + 1; // Set the image to be either the next image in the list, or 0.
                    GameDebug.WriteLine(_nextImage);
                    _transitionMenuImage = _images[_nextImage]; // Set the transition image, ready for the next transition.
                    _transitionMenuImage.Reset(); // Reset any values in the transition image.
                    // When the image's max scale is reached, transition is set to true
                    _activeMenuImage.MaxScaleReached += (sender, args) => { _transition = true; };
                    _startingSeconds = default; // Reset the starting seconds ready for next transition.
                }
                else
                {
                    // Lerp between alpha values 0 and 1 over the span of 3 seconds.
                    _transitionMenuImage.Alpha = MathHelper.Lerp(0, 1,
                        ((float) gameTime.TotalGameTime.TotalSeconds - _startingSeconds) / 3);
                    // Update the transition image too, allowing it to zoom in even while fading.
                    _transitionMenuImage.Update(gameTime);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.Black);

            _transformMatrix = Matrix.CreateScale(new Vector3(
                (float) SE2.Graphics.PreferredBackBufferWidth / 1280,
                (float) SE2.Graphics.PreferredBackBufferHeight / 720, 1));
            GraphicsDevice.RasterizerState = _rasterizerState;
            spriteBatch.Begin(SpriteSortMode.Immediate, transformMatrix: _transformMatrix, rasterizerState: _rasterizerState);
            _activeMenuImage.Draw(spriteBatch);
            _transitionMenuImage.Draw(spriteBatch);
            //_playButton.Draw(spriteBatch);
            //_quitButton.Draw(spriteBatch);
            //_text.Draw(spriteBatch);
            UIManager.Draw(spriteBatch);
            spriteBatch.End();
        }
        
        private void OpenPanel(string panelName)
        {
            UIElement panel = UIManager.GetElement(panelName);
            
            foreach ((_, UIElement element) in UIManager.Elements)
                if (element.Parent != panel)
                    element.Active = false;

            panel.Show = true;
            UIManager.GetElement("backgroundPanel").Show = true;
            GameDebug.WriteLine($"Opening panel: {panelName}");
        }
        
        private void ClosePanel(string panelName)
        {
            UIManager.GetElement(panelName).Show = false;
            UIManager.GetElement("backgroundPanel").Show = false;
            
            foreach ((_, UIElement element) in UIManager.Elements)
                element.Active = true;
            GameDebug.WriteLine($"Closing panel: {panelName}");
        }
    }

    public class MenuImage
    {
        /// <summary>
        /// The menu image's texture.
        /// </summary>
        public Texture2D Texture;
        
        /// <summary>
        /// The menu image's position.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The alpha value of the menu image (between 0 and 1)
        /// </summary>
        public float Alpha;

        /// <summary>
        /// The scale of the image when it is transitioned to
        /// </summary>
        public float InitialScale = 0.85f;
        /// <summary>
        /// The scale the image must reach before it calls <see cref="MaxScaleReached"/>
        /// </summary>
        public float MaxScale = 1.25f;
        
        private float _scaleMultiplier; // The scale of the image on-screen.

        /// <summary>
        /// The minimum time in seconds for which the slide will display. Does not include the transition time.
        /// </summary>
        public float ScaleSpeed = 30;

        private bool _getSeconds; // If true, it will get the current game time in seconds.
        private float _startingSeconds; // THe current game time in seconds, used to calculate the time the transition stared.

        private bool _invoked; // To prevent invoking the transition event many times, this is immediately set "true" once the event is invoked.

        /// <summary>
        /// Invoked when the <see cref="MaxScale"/> is reached.
        /// </summary>
        public event OnMaxScaleReached MaxScaleReached;
        
        public MenuImage(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            Alpha = 0;
            _scaleMultiplier = InitialScale;
            _getSeconds = true;
        }

        public void Update(GameTime gameTime)
        {
            if (_getSeconds) // Get the current game time in seconds.
            {
                _getSeconds = false;
                _startingSeconds = (float) gameTime.TotalGameTime.TotalSeconds;
            }

            // Calculate the scale value of the image, using linear interpolation (lerp)
            _scaleMultiplier = MathHelper.Lerp(InitialScale, MaxScale,
                ((float) gameTime.TotalGameTime.TotalSeconds - _startingSeconds) / ScaleSpeed);

            // Runs once the scale reaches the max scale.
            if (_scaleMultiplier >= MaxScale)
            {
                if (!_invoked)
                {
                    _invoked = true;
                    MaxScaleReached?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Reset scale values of the image, so it can be used again.
        /// </summary>
        public void Reset()
        {
            _getSeconds = true;
            _invoked = false;
            _scaleMultiplier = InitialScale;
            Alpha = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White * Alpha, 0,
                new Vector2(Texture.Width, Texture.Height) / 2, _scaleMultiplier, SpriteEffects.None, 0);
        }
    }

    public delegate void OnMaxScaleReached(object sender, EventArgs args);
}