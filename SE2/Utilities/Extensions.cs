using Microsoft.Xna.Framework;

namespace SE2.Utilities
{
    public static class Extensions
    {
        public static Size ToSize(this Vector2 value)
        {
            return new Size(value.X, value.Y);
        }

        public static System.Numerics.Vector3 ToSysVec3(this Vector3 vector)
        {
            return new System.Numerics.Vector3(vector.X, vector.Y, vector.Z);
        }
    }
}