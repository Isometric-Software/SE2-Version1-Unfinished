using System;
using BepuPhysics;
using BepuPhysics.Collidables;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE2.Data;
using SE2.Scenes;
using SE2.Utilities;
using SVec3 = System.Numerics.Vector3;

namespace SE2.Entities
{
    public class Player : Entity
    {
        private float _movementSpeed = 6;
        private float _sprintSpeed = 10;
        private float _crouchSpeed = 2;
        private float _speed;

        private BodyHandle _bodyHandle;
        private BodyReference _currentRef;

        private MouseState _last;

        public Player(Vector3 position, Quaternion rotation)
            : base(position, rotation, null, show: false)
        {
            Capsule box = new Capsule(1, 2);
            box.ComputeInertia(1, out BodyInertia inertia);
            _bodyHandle = MainScene.Simulation.Bodies.Add(BodyDescription.CreateDynamic(position.ToSysVec3(), inertia,
                new CollidableDescription(MainScene.Simulation.Shapes.Add(box), 0.1f),
                new BodyActivityDescription(0.01f)));
        }
        
        public override void Update(GameTime gameTime, KeyboardState kState, MouseState mState)
        {
            if (!SE2.Active) return;
            _speed = _movementSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds;
            
            _last = MainScene.LastMouseState;
            
            KeysConfig GKeys = GameConfig.LoadedKeysConfig;
            
            _currentRef = MainScene.Simulation.Bodies.GetBodyReference(_bodyHandle);
            
            if (kState.IsKeyDown(GKeys.Forward)) Move(z: -_speed);
            if (kState.IsKeyDown(GKeys.Backward)) Move(z: _speed);
            if (kState.IsKeyDown(GKeys.Left)) Move(-_speed);
            if (kState.IsKeyDown(GKeys.Right)) Move(_speed);
            if (kState.IsKeyDown(GKeys.Jump)) Move(y: _speed);
            if (kState.IsKeyDown(GKeys.Crouch)) Move(y: -_speed);
            
            Rotation.X += (_last.X - mState.X) * 0.005f;
            Rotation.Y += (_last.Y - mState.Y) * 0.005f;

            Rotation.Y = Math.Clamp(Rotation.Y, -MathHelper.ToRadians(89.9f), MathHelper.ToRadians(89.9f));
            
            //SVec3 pos = _currentRef.Pose.Position;
            //Position = new Vector3(pos.X, pos.Y, pos.Z);
        }

        private void Move(Vector3 position)
        {
            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(Rotation.X, Rotation.Y, Rotation.Z);
            Position += Vector3.Transform(position, rotationMatrix);
        }

        private void Move(float x = 0, float y = 0, float z = 0) => Move(new Vector3(x, y, z));
    }
}