using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE2.Utilities;

namespace SE2.UI
{
    public class UIPanel : UIElement
    {
        private UIRectangle _rectangle;
        private UIBorderRectangle _border;

        public new Color Color
        {
            set
            {
                if (_rectangle != null) _rectangle.Color = value;
            }
        }

        public UIPanel(GraphicsDevice graphicsDevice, Vector2 position, Size size, Color color, int borderWidth = 0, Color borderColor = default) 
            : base(graphicsDevice, position, size)
        {
            Color = color;
            _rectangle = new UIRectangle(graphicsDevice, position, size, color);
            _border = new UIBorderRectangle(graphicsDevice, position, size, borderWidth, borderColor);
        }

        public override void Update(GameTime gameTime, KeyboardState kState, MouseState mState, Vector2 mousePos = default)
        {
            _rectangle.Position = Position;
            _border.Position = Position;
            
            base.Update(gameTime, kState, mState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _rectangle.Draw(spriteBatch);
            _border.Draw(spriteBatch);
        }
    }
}