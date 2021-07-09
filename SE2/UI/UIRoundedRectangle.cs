using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SE2.Utilities;

namespace SE2.UI
{
    public class UIRoundedRectangle : UIElement
    {
        private Texture2D _texture;
        
        public UIRoundedRectangle(GraphicsDevice graphicsDevice, Vector2 position, Size size) : base(graphicsDevice, position, size)
        {
            _texture = new Texture2D(graphicsDevice, 200, 200);

            Vector2 d1 = new Vector2(200 - 150, 200 - 0);
            Vector2 d2 = new Vector2(200 - 200, 200 - 50);

            float radius = 20;
            
            double angle = (Math.Atan2(d1.Y, d1.X) - Math.Atan2(d2.Y, d2.X)) / 2;

            double tan = Math.Abs(Math.Tan(angle));
            double segment = radius / tan;

            double length1 = GetLength(d1);
            double length2 = GetLength(d2);

            double length = Math.Min(length1, length2);

            if (segment > length)
            {
                segment = length;
                radius = (float) (length * tan);
            }
            
            //Vector2 p1Cross = GetProportionVector()
        }

        private double GetLength(Vector2 vector)
        {
            return vector.X * vector.X + vector.Y * vector.Y;
        }

        private Vector2 GetProportionVector(Vector2 vector, double segment, double length, Vector2 d)
        {
            double factor = segment / length;

            return new Vector2((float) (vector.X - d.X * factor), (float) (vector.Y - d.Y * factor));

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}