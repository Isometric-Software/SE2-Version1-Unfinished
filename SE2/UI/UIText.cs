using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE2.Utilities;

namespace SE2.UI
{
    public class UIText : UIElement
    {
        public SpriteFont Font;
        public string Text { get; set; }

        public int FontSize;

        private Vector2 _alignment;

        private Vector2 MeasureText => Font.MeasureString(Text);

        public TextAlignment Alignment
        {
            set
            {
                switch (value)
                {
                    case TextAlignment.TopLeft:
                        _alignment = new Vector2(0, 0);
                        break;
                    case TextAlignment.TopMiddle:
                        _alignment = new Vector2(MeasureText.X / 2, 0);
                        break;
                    case TextAlignment.TopRight:
                        _alignment = new Vector2(MeasureText.X, 0);
                        break;
                    case TextAlignment.CenterLeft:
                        _alignment = new Vector2(0, MeasureText.Y / 2);
                        break;
                    case TextAlignment.CenterMiddle:
                        _alignment = new Vector2(MeasureText.X, MeasureText.Y) / 2;
                        break;
                    case TextAlignment.CenterRight:
                        _alignment = new Vector2(MeasureText.X, MeasureText.Y / 2);
                        break;
                    case TextAlignment.BottomLeft:
                        _alignment = new Vector2(0, MeasureText.Y);
                        break;
                    case TextAlignment.BottomMiddle:
                        _alignment = new Vector2(MeasureText.X / 2, MeasureText.Y);
                        break;
                    case TextAlignment.BottomRight:
                        _alignment = MeasureText;
                        break;
                }
            }
        }

        private int _size;
        
        public float Scale => (float) FontSize / _size;

        public UIText(Vector2 position, string fontName, string text, int fontSize = 24, bool useSize256 = true) : base(null, position, default)
        {
            Text = text;
            Color = Color.White;
            FontSize = fontSize;
            if (useSize256)
            {
                Font = FontManager.GetFont(fontName + "-256px");
                _size = 256;
            }
            else
            {
                Font = FontManager.GetFont(fontName);
                _size = int.Parse(fontName.Split('-')[1].Split("px")[0]);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position, Color, 0, _alignment, Scale, SpriteEffects.None, 0);
        }
    }

    public enum TextAlignment
    {
        TopLeft, TopMiddle, TopRight,
        CenterLeft, CenterMiddle, CenterRight,
        BottomLeft, BottomMiddle, BottomRight
    }
}