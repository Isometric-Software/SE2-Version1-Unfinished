using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE2.Debug;
using SE2.Scenes;
using SE2.Utilities;

namespace SE2.Entities
{
    public class egg : Entity
    {
        private BodyHandle _bodyHandle;
        public egg(Vector3 position, Quaternion rotation, Model model, Texture2D texture = null, Vector3 scale = default,
            bool show = true)
            : base(position, rotation, model, texture, scale, show)
        {
            Box box = new Box(2, 2, 2);
            box.ComputeInertia(1, out BodyInertia inertia);
            _bodyHandle = MainScene.Simulation.Bodies.Add(BodyDescription.CreateDynamic(position.ToSysVec3(), inertia,
                new CollidableDescription(MainScene.Simulation.Shapes.Add(box), 0.1f),
                new BodyActivityDescription(0.01f)));
        }
        public override void Update(GameTime gameTime, KeyboardState kState, MouseState mState)
        {
            BodyReference reference = MainScene.Simulation.Bodies.GetBodyReference(_bodyHandle);
            System.Numerics.Vector3 position = reference.Pose.Position;
            System.Numerics.Quaternion rotation = reference.Pose.Orientation;
            Position = new Vector3(position.X, position.Y, position.Z);
            Rotation = new Quaternion(rotation.X, rotation.Y, rotation.Z, rotation.W);
            if (Position.Y < -10) EntityManager.Remove(this);
        }
    }
}