using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace SE2.Utilities
{
    public static class FontManager
    {
        private static Dictionary<string, SpriteFont> _fonts = new Dictionary<string, SpriteFont>();

        public static void AddFont(string fontName, SpriteFont font)
        {
            _fonts.Add(fontName, font);
        }

        public static SpriteFont GetFont(string fontName)
        {
            return _fonts[fontName];
        }
    }
}