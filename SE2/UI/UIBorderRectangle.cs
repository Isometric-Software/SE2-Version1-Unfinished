using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE2.Utilities;

namespace SE2.UI
{
    public class UIBorderRectangle : UIElement
    {
        private Texture2D _texture;
        
        public UIBorderRectangle(GraphicsDevice graphicsDevice, Vector2 position, Size size, int borderWidth, Color color) 
            : base(graphicsDevice, position, size)
        {
            Color = color;
            GenerateTexture(borderWidth, size);
        }

        private void GenerateTexture(int borderWidth, Size size)
        {
            _texture?.Dispose();
            _texture = new Texture2D(GraphicsDevice, (int) size.Width, (int) size.Height);
            Color[] pixels = new Color[(int) (size.Width * size.Height)];
            for (int x = 0; x < _texture.Width; x++)
            {
                for (int y = 0; y < _texture.Height; y++)
                {
                    if (x <= borderWidth || y <= borderWidth || x >= _texture.Width - borderWidth ||
                        y >= _texture.Height - borderWidth)
                        pixels[y * _texture.Width + x] = Color.White;
                    else
                        pixels[y * _texture.Width + x] = Color.Transparent;
                }
            }

            _texture.SetData(pixels);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, Color);
        }
    }
}