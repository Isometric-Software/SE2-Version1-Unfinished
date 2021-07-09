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
    public class GameObject : Entity
    {
        private StaticHandle _staticHandle;
        public GameObject(Vector3 position, Quaternion rotation, Model model, Texture2D texture = null,
            Vector3 scale = default, bool show = true, bool isStatic = false)
            : base(position, rotation, model, texture, scale, show)
        {
            Box box = new Box(2, 2, 2);
            _staticHandle = MainScene.Simulation.Statics.Add(new StaticDescription(position.ToSysVec3(),
                    new CollidableDescription(MainScene.Simulation.Shapes.Add(box), 0.1f)));
        }

        public override void Update(GameTime gameTime, KeyboardState kState, MouseState mState)
        {
            if (Position.Y < -10) EntityManager.Remove(this);
        }
    }
}