using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE2.Debug;
using SE2.Utilities;

namespace SE2.UI
{
    public class UIButton : UIElement
    {
        private UIPanel _panel;
        private UIText _text;

        public Color HoverColor;

        public event ButtonClicked OnClick;

        private bool _clicked;

        public string Text
        {
            get => _text.Text;
            set => _text.Text = value;
        }

        public UIButton(GraphicsDevice graphicsDevice, Vector2 position, Size size, Color backColor, string text = "",
            int borderWidth = 0, Color borderColor = default, Color textColor = default, int fontSize = 24)
                : base(graphicsDevice, position, size)
        {
            Color = backColor;
            _panel = new UIPanel(graphicsDevice, position, size, backColor, borderWidth, borderColor);
            _text = new UIText(new Vector2(position.X + size.Width/2, position.Y + size.Height/2), "Arial", text, fontSize)
            {
                Color = textColor,
                Alignment = TextAlignment.CenterMiddle
            };
            HoverColor = Color;
        }

        public override void Update(GameTime gameTime, KeyboardState kState, MouseState mState, Vector2 mousePos = default)
        {
            base.Update(gameTime, kState, mState);

            if (!Show) return;
            
            _panel.Position = Position;
            _text.Position = new Vector2(Position.X + Size.Width / 2, Position.Y + Size.Height / 2);
            
            _panel.Update(gameTime, kState, mState, mousePos);
            
            if (mousePos.X >= Position.X && mousePos.X <= Position.X + Size.Width && mousePos.Y >= Position.Y &&
                mousePos.Y <= Position.Y + Size.Height && Active)
            {
                _panel.Color = HoverColor;
                if (mState.LeftButton == ButtonState.Pressed)
                {
                    if (!_clicked)
                    {
                        GameDebug.WriteLine("Button click!");
                        _clicked = true;
                        OnClick?.Invoke(this, EventArgs.Empty);
                    }
                }
                else _clicked = false;
            }
            else
            {
                _panel.Color = Color;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _panel.Draw(spriteBatch);
            _text.Draw(spriteBatch);
        }
    }

    public delegate void ButtonClicked(object sender, EventArgs args);
}