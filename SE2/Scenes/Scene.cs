using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SE2.Scenes
{
    public abstract class Scene
    {
        protected SE2 SE2;
        protected ContentManager Content;
        protected GraphicsDevice GraphicsDevice;

        public Scene(SE2 se2)
        {
            SE2 = se2;
            Content = se2.Content;
            GraphicsDevice = se2.GraphicsDevice;
        }

        public virtual void Initialize()
        {
            LoadContent();
        }

        protected virtual void LoadContent()
        {
            
        }

        public virtual void UnloadContent()
        {
            
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}