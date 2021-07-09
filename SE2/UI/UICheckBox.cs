using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE2.Debug;
using SE2.Utilities;

namespace SE2.UI
{
    public class UICheckBox : UIButton
    {
        public event BoxChecked OnCheck;
        
        private bool _checked;

        private UIRectangle _checkedRectangle;

        public int BoxOffset
        {
            set => _checkedRectangle.Size = Size - new Size(value, value) * 2;
        }
        
        public UICheckBox(GraphicsDevice graphicsDevice, Vector2 position, Size size, Color backColor,
            Color checkedColor, int borderWidth = 0, Color borderColor = default, bool isChecked = false) : base(graphicsDevice, position, size,
            backColor, borderWidth: borderWidth, borderColor: borderColor)
        {
            _checkedRectangle = new UIRectangle(graphicsDevice, position, size , checkedColor);
            _checkedRectangle.Origin = new Vector2(_checkedRectangle.Width, _checkedRectangle.Height) / 2;
            _checkedRectangle.Position = position + size.ToVector2() / 2;
            
            _checked = isChecked;
            
            OnClick += (sender, args) =>
            {
                _checked = !_checked;
                OnCheck?.Invoke(this, new CheckBoxEventArgs(_checked));
            };

            BoxOffset = 10;
        }

        public override void Update(GameTime gameTime, KeyboardState kState, MouseState mState, Vector2 mousePos = default)
        {
            base.Update(gameTime, kState, mState, mousePos);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (_checked)
                _checkedRectangle.Draw(spriteBatch);
        }
    }

    public class CheckBoxEventArgs : EventArgs
    {
        public bool Checked
        {
            get;
            set;
        }
        
        public CheckBoxEventArgs(bool isChecked)
        {
            Checked = isChecked;
        }
    }

    public delegate void BoxChecked(object sender, CheckBoxEventArgs args);
}