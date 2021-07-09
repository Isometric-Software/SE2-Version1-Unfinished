using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE2.Utilities;

namespace SE2.UI
{
    public class UICloseButton : UIButton
    {
        private Texture2D _xTexture;

        private Color _textureColor;
        
        public UICloseButton(GraphicsDevice graphicsDevice, Vector2 position, Size size, Color color, int width = 2) : base(graphicsDevice,
            position, size, Color.Transparent)
        {
            Color = color;
            _xTexture = new Texture2D(graphicsDevice, (int) size.Width, (int) size.Height);
            Color[] pixels = new Color[(int) (size.Width * size.Height)];
            for (int x = 0; x < _xTexture.Width; x++)
            {
                for (int y = 0; y < _xTexture.Height; y++)
                {
                    int nx = _xTexture.Width - x;
                    if (x >= y && x <= y + width/2 || x <= y && x >= y - width/2 || nx >= y && nx <= y + width/2 || nx <= y && nx >= y - width/2) 
                        pixels[y * _xTexture.Width + x] = Color.White;
                }
            }
            _xTexture.SetData(pixels);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            spriteBatch.Draw(_xTexture, Position, Color);
        }
    }
}