using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE2.Debug;
using SE2.Utilities;

namespace SE2.UI
{
    public class UiDropDown : UIButton
    {
        public event SelectionChanged OnSelectionChanged;
        
        private List<UIDropDownItem> _items;

        private int _selectedItem;

        public UIDropDownItem SelectedItem => _items[_selectedItem];

        private bool _opened;

        private int _lastScrollWheelValue;

        private Rectangle _clipper;
        
        public UiDropDown(GraphicsDevice graphicsDevice, Vector2 position, Size size, Color backColor, string firstItemText,
            int borderWidth = 0, Color borderColor = default, Color textColor = default, int fontSize = 24) : base(
            graphicsDevice, position, size, backColor, "NO_ITEMS", borderWidth, borderColor, Color.Black, fontSize)
        {
            _items = new List<UIDropDownItem>();
            _items.Add(new UIDropDownItem(graphicsDevice, firstItemText, this, position + new Vector2(0, size.Height),
                size));
            _selectedItem = 0;
            _opened = false;

            Size gSize = new Size(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            
            _clipper = new Rectangle((int) position.X, (int) (position.Y + size.Height), (int) size.Width, 200);
            _clipper.X= (int)(_clipper.X * (gSize.Width / 1280));
            _clipper.Y= (int)(_clipper.Y * (gSize.Height / 720));
            _clipper.Width= (int)(_clipper.Width * (gSize.Width / 1280));
            _clipper.Height= (int)(_clipper.Height * (gSize.Height / 720));

            GameDebug.WriteLine(_clipper);
            
            OnClick += (sender, args) => _opened = !_opened;
            OnSelectionChanged += (sender, args) =>
            {
                _opened = false;
                int i = 0;
                foreach (UIDropDownItem item in _items)
                {
                    if (i == 0)
                        item.Position = Position + new Vector2(0, Size.Height);
                    else
                        item.Position = _items[i - 1].Position + new Vector2(0, _items[i - 1].Size.Height);
                    i++;
                }
            };
        }

        public void Add(string itemText)
        {
            _items.Add(new UIDropDownItem(GraphicsDevice, itemText, this,
                _items[^1].Position + new Vector2(0, Size.Height), Size));
        }

        public override void Update(GameTime gameTime, KeyboardState kState, MouseState mState, Vector2 mousePos = default)
        {
            base.Update(gameTime, kState, mState, mousePos);

            if (_opened)
            {
                foreach (UIDropDownItem item in _items)
                    item.Update(gameTime, kState, mState, mousePos);

                if (mState.ScrollWheelValue < _lastScrollWheelValue)
                {
                    foreach (UIDropDownItem item in _items)
                        item.Position.Y -= 10;
                }
                else if (mState.ScrollWheelValue > _lastScrollWheelValue)
                {
                    foreach (UIDropDownItem item in _items)
                        item.Position.Y += 10;
                }

                _lastScrollWheelValue = mState.ScrollWheelValue;
            }

            Text = _items[_selectedItem].Text;
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (!_opened) return;
            Rectangle currentRect = spriteBatch.GraphicsDevice.ScissorRectangle;
            spriteBatch.GraphicsDevice.ScissorRectangle = _clipper;
            foreach (UIDropDownItem item in _items)
                item.Draw(spriteBatch);
            spriteBatch.GraphicsDevice.ScissorRectangle = currentRect;
        }

        public void ChangeItem(UIDropDownItem item)
        {
            _selectedItem = _items.IndexOf(item);
            OnSelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public delegate void SelectionChanged(object sender, EventArgs args);

    public class UIDropDownItem : UIButton
    {
        private UiDropDown _parentDropDown;

        public UIDropDownItem(GraphicsDevice graphicsDevice, string text, UiDropDown parentDropDown, Vector2 position, Size size) : base(graphicsDevice, position, size,
            parentDropDown.Color, text, textColor: Color.Black)
        {
            _parentDropDown = parentDropDown;
            
            OnClick += (sender, args) => _parentDropDown.ChangeItem(this);
        }

        public override void Update(GameTime gameTime, KeyboardState kState, MouseState mState, Vector2 mousePos = default)
        {
            base.Update(gameTime, kState, mState, mousePos);
            
        }
    }
}