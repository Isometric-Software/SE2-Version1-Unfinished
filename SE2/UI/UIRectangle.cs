using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE2.Utilities;

namespace SE2.UI
{
    public class UIRectangle : UIElement
    {
        private Texture2D _texture { get; }
        public int Width => _texture.Width;
        public int Height => _texture.Height;

        public UIRectangle(GraphicsDevice graphicsDevice, Vector2 position, Size size, Color color) 
            : base(graphicsDevice, position, size)
        {
            Color = color;
            _texture = new Texture2D(graphicsDevice, 1, 1);
            Color[] singleColor = new Color[] { Color.White };
            _texture.SetData(singleColor);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color, Rotation, Origin, Size.ToVector2(),
                SpriteEffects.None, 0);
        }
    }
}