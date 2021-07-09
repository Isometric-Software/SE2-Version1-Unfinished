using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE2.Debug;

namespace SE2.Scenes
{
    public class IntroScene : Scene
    {
        public IntroScene(SE2 se2) : base(se2) { }

        private Texture2D _logo;

        private float _alpha;
        
        protected override void LoadContent()
        {
            _logo = Content.Load<Texture2D>("Images/isometric software logo");

            GameDebug.WriteLine("Starting SE2!\nOllie Robinson 2021");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float seconds = (float) gameTime.TotalGameTime.TotalSeconds;
            if (seconds < 2)
                _alpha = MathHelper.Lerp(0, 1, seconds - 1);
            else
                _alpha = MathHelper.Lerp(0, 1, 5 - seconds);
            if (seconds > 6)
                SE2.LoadScene(new MenuScene(SE2, true));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.Black);
            Vector2 graphicsBackBuffer = new Vector2(SE2.Graphics.PreferredBackBufferWidth,
                SE2.Graphics.PreferredBackBufferHeight);
            spriteBatch.Begin(transformMatrix: Matrix.CreateScale(new Vector3(graphicsBackBuffer.X / 1280, graphicsBackBuffer.Y / 720, 1)));
            spriteBatch.Draw(_logo, Vector2.Zero, null, Color.White * _alpha, 0, Vector2.Zero,
                new Vector2(1280 / (float) _logo.Width, 720 / (float) _logo.Height), SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}