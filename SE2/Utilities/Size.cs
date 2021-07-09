using System;
using Microsoft.Xna.Framework;

namespace SE2.Utilities
{
    public struct Size : IEquatable<Size>
    {
        public float Width;
        public float Height;

        public Size(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public bool Equals(Size other)
        {
            return Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        public override bool Equals(object obj)
        {
            return obj is Size other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Width, Height);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(Width, Height);
        }

        public override string ToString()
        {
            return "{Width: " + Width + ", Height: " + Height + "}";
        }

        public static Size operator +(Size size1, Size size2)
        {
            return new Size(size1.Width + size2.Width, size1.Height + size2.Height);
        }
        
        public static Size operator -(Size size1, Size size2)
        {
            return new Size(size1.Width - size2.Width, size1.Height - size2.Height);
        }
        
        public static Size operator /(Size size1, Size size2)
        {
            return new Size(size1.Width / size2.Width, size1.Height / size2.Height);
        }
        
        public static Size operator /(Size size1, float value)
        {
            return new Size(size1.Width / value, size1.Height / value);
        }
        
        public static Size operator *(Size size1, Size size2)
        {
            return new Size(size1.Width * size2.Width, size1.Height * size2.Height);
        }
        
        public static Size operator *(Size size1, float value)
        {
            return new Size(size1.Width * value, size1.Height * value);
        }
    }
}