using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SE2.UI
{
    public static class UIManager
    {
        public static Dictionary<string, UIElement> Elements = new Dictionary<string, UIElement>();

        public static void Update(GameTime gameTime, KeyboardState kState, MouseState mState, Vector2 mousePosition)
        {
            foreach ((_, UIElement element) in Elements)
            {
                //if (element.Show)
                element.Update(gameTime, kState, mState, mousePosition);
            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach ((_, UIElement element) in Elements)
            {
                if (element.Show)
                    element.Draw(spriteBatch);
            }
        }
        
        /// <summary>
        /// Add a new UI element to the manager.
        /// </summary>
        /// <param name="elementName">The name of the element. MUST be unique.</param>
        /// <param name="element">The UI element.</param>
        public static void Add(string elementName, UIElement element)
        {
            Elements.Add(elementName, element);
        }

        public static void Clear()
        {
            Elements.Clear();
        }

        public static UIElement GetElement(string elementName)
        {
            return Elements[elementName];
        }
    }
}