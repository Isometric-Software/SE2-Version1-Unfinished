using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE2.Debug;
using SE2.Utilities;

namespace SE2.UI.Panels
{
    public class MessageBoxPanel : UIElement
    {
        private UIPanel _panel;
        private UIText _text;
        private List<UIButton> _buttons;
        
        public MessageBoxPanel(GraphicsDevice graphicsDevice, Vector2 position, Size size, string text, MessageBoxButtons buttons = MessageBoxButtons.None) : base(graphicsDevice, position, size)
        {
            SpriteFont font = FontManager.GetFont("Arial-256px");
            string[] words = text.Split(" ");
            string tempText = "";
            for (int i = 0; i < text.Length; i++)
            {
                char character = text[i];
                Vector2 textSize = font.MeasureString(tempText + character) * ((float)24 / 256);
                if (textSize.X > size.Width - 10)
                    tempText += "\n";
                GameDebug.WriteLine(textSize.X);
                tempText += character;
            }
            _text = new UIText(position + new Vector2(size.Width / 2, 10), "Arial", tempText) {Alignment = TextAlignment.TopMiddle};
            _panel = new UIPanel(graphicsDevice, position, size, Color.Black);
            _buttons = new List<UIButton>();
            switch (buttons)
            {
                case MessageBoxButtons.Cancel:
                    _buttons.Add(new UIButton(graphicsDevice,
                        new Vector2(position.X + size.Width / 2 - 50, position.Y + size.Height - 60), new Size(100, 50),
                        Color.Gray, "Cancel", 2, Color.White, Color.White, 18));
                    break;
                case MessageBoxButtons.Ok:
                    _buttons.Add(new UIButton(graphicsDevice,
                        new Vector2(position.X + size.Width / 2 - 50, position.Y + size.Height - 60), new Size(100, 50),
                        Color.Gray, "OK", 2, Color.White, Color.White, 18));
                    break;
                case MessageBoxButtons.OkCancel:
                    _buttons.Add(new UIButton(graphicsDevice,
                        new Vector2(position.X + size.Width / 2 - 110, position.Y + size.Height - 60), new Size(100, 50),
                        Color.Gray, "OK", 2, Color.White, Color.White, 18));
                    _buttons.Add(new UIButton(graphicsDevice,
                        new Vector2(position.X + size.Width / 2 + 10, position.Y + size.Height - 60), new Size(100, 50),
                        Color.Gray, "Cancel", 2, Color.White, Color.White, 18));
                    break;
                case MessageBoxButtons.YesNo:
                    _buttons.Add(new UIButton(graphicsDevice,
                        new Vector2(position.X + size.Width / 2 - 110, position.Y + size.Height - 60), new Size(100, 50),
                        Color.Gray, "Yes", 2, Color.White, Color.White, 18));
                    _buttons.Add(new UIButton(graphicsDevice,
                        new Vector2(position.X + size.Width / 2 + 10, position.Y + size.Height - 60), new Size(100, 50),
                        Color.Gray, "No", 2, Color.White, Color.White, 18));
                    break;
                case MessageBoxButtons.OkApplyCancel:
                    _buttons.Add(new UIButton(graphicsDevice,
                        new Vector2(position.X + size.Width / 2 - 160, position.Y + size.Height - 60), new Size(100, 50),
                        Color.Gray, "OK", 2, Color.White, Color.White, 18));
                    _buttons.Add(new UIButton(graphicsDevice,
                        new Vector2(position.X + size.Width / 2 - 50, position.Y + size.Height - 60), new Size(100, 50),
                        Color.Gray, "Apply", 2, Color.White, Color.White, 18));
                    _buttons.Add(new UIButton(graphicsDevice,
                        new Vector2(position.X + size.Width / 2 + 60, position.Y + size.Height - 60), new Size(100, 50),
                        Color.Gray, "Cancel", 2, Color.White, Color.White, 18));
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _panel.Draw(spriteBatch);
            _text.Draw(spriteBatch);
            foreach(UIButton button in _buttons)
                button.Draw(spriteBatch);
        }
    }

    public enum MessageBoxButtons
    {
        None,
        Ok,
        OkCancel,
        Cancel,
        OkApplyCancel,
        YesNo
    }
}