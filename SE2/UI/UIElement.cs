using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE2.Utilities;

namespace SE2.UI
{
    public abstract class UIElement
    {
        public UIElement Parent; // The parent UI element. This element copies certain values of the parent.
        
        public Vector2 Position; // Where the element is located.
        public Size Size; // The size of the element.
        protected readonly GraphicsDevice GraphicsDevice; // The graphics device, used in case textures need to be changed later.
        public Color Color; // The colour of the element.

        public bool Show; // If false, the element will not display.
        public bool Active; // The element will still show, however it will always be in an idle state, as long as this is true.

        public Vector2 Origin;
        public float Rotation;
        
        public UIElement(GraphicsDevice graphicsDevice, Vector2 position, Size size)
        {
            Position = position;
            Size = size;
            GraphicsDevice = graphicsDevice;
            Show = true;
            Active = true;
        }

        public UIElement(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
            Show = true;
            Active = true;
        }

        public virtual void Update(GameTime gameTime, KeyboardState kState, MouseState mState, Vector2 mousePos = default)
        {
            if (Parent != null)
            {
                Show = Parent.Show;
            }

            if (!Show) return;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}