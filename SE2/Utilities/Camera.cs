using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SE2.Utilities
{
    public class Camera
    {
        public Vector3 Position;
        public Vector3 Rotation;
        
        public Matrix ProjectionMatrix { get; }

        public Matrix ViewMatrix
        {
            get
            {
                Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z);
                return Matrix.CreateLookAt(Position, Position + Vector3.Transform(Vector3.Forward, rotationMatrix),
                    Vector3.Up);
            }
        }

        public Vector3 LookDirection
        {
            get
            {
                Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z);
                return Vector3.Transform(Vector3.Forward, rotationMatrix);
            }
        }

        public Camera(GraphicsDevice graphicsDevice, Vector3 position, Vector3 rotation, float fov = 45, float near = 0.1f, float far = 1000f)
        {
            Position = position;
            Rotation = rotation;
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov),
                graphicsDevice.Viewport.AspectRatio, near, far);
        }
    }
}