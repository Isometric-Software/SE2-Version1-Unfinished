using System;
using System.Collections.Generic;
using BepuPhysics;
using BepuUtilities;
using BepuUtilities.Memory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SE2.Debug;
using SE2.Entities;
using SE2.Physics;
using SE2.Utilities;
using Matrix = Microsoft.Xna.Framework.Matrix;
using SVec3 = System.Numerics.Vector3;

namespace SE2.Scenes
{
    public class MainScene : Scene
    {
        public static Player Player;

        public static MouseState LastMouseState;
        
        public static Camera Camera;
        
        private Matrix _world;

        private bool _escapeHeld;

        private GameObject _placeCube;

        private BufferPool _bufferPool;

        public static Simulation Simulation;

        private bool _leftPressed;

        private Model _cube;
        private Texture2D _grass;
        private Texture2D _wood;

        private int _blockDistance;

        public MainScene(SE2 se2) : base(se2)
        {
            se2.IsMouseVisible = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            
            
            _cube = Content.Load<Model>("Primitives/IMyDefaultCube");
            _grass = Content.Load<Texture2D>("Textures/grass");
            _wood = Content.Load<Texture2D>("Textures/wood");
            
            _bufferPool = new BufferPool();
            Simulation = Simulation.Create(_bufferPool, new NarrowPhaseCallbacks(),
                new PoseIntegratorCallbacks(new SVec3(0, -9.81f, 0)), new PositionLastTimestepper());
            
            Player = new Player(new Vector3(4, 5, 4), new Quaternion(0.7854f, -0.7854f, 0, 0));

            for (int x = -50; x < 50; x+= 2)
                for (int y = -50; y < 50; y += 2)
                    EntityManager.Add(new GameObject(new Vector3(x, 0, y), new Microsoft.Xna.Framework.Quaternion(0, 0, 0, 0), _cube, _grass, isStatic: true));
            //EntityManager.Add(new GameObject(Vector3.Zero, Vector3.Zero, cube, grass, new Vector3(100, 1, 100)));

            _placeCube = new GameObject(Vector3.Zero, new Microsoft.Xna.Framework.Quaternion(0, 0, 0, 0), _cube, _wood);
            
            _world = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
            Camera = new Camera(GraphicsDevice, new Vector3(4, 5, 4), new Vector3(0.7854f, -0.7854f, 0));

            _blockDistance = 10;

            GameDebug.WriteLine($"Loading main game - Processing {EntityManager.EntityCount} entities.");
        }

        public override void UnloadContent()
        {
            EntityManager.Clear();
            Simulation.Dispose();
            _bufferPool.Clear();
            
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();
            MouseState mState = Mouse.GetState();

            if (SE2.IsActive)
            {
                if (Math.Abs((float) mState.X / 2 / ((float) SE2.Graphics.PreferredBackBufferWidth / 2) - 0.5f) * 2 >
                    0.75 || Math.Abs((float) mState.Y / 2 / ((float) SE2.Graphics.PreferredBackBufferHeight / 2) - 0.5f) *
                    2 > 0.75)
                {
                    Mouse.SetPosition(SE2.Graphics.PreferredBackBufferWidth / 2, SE2.Graphics.PreferredBackBufferHeight / 2);
                    LastMouseState = Mouse.GetState();
                    mState = LastMouseState;
                }
            }

            _placeCube.Update(gameTime, kState, mState);
            Player.Update(gameTime, kState, mState);

            Camera.Position = Player.Position;
            Camera.Rotation = new Vector3(Player.Rotation.X, Player.Rotation.Y, Player.Rotation.Z);

            if (mState.ScrollWheelValue > LastMouseState.ScrollWheelValue)
                _blockDistance++;
            else if (mState.ScrollWheelValue < LastMouseState.ScrollWheelValue)
                _blockDistance--;

            _blockDistance = Math.Clamp(_blockDistance, 3, 15);
            
            _placeCube.Position = Player.Position + Camera.LookDirection * _blockDistance;

            if (mState.LeftButton == ButtonState.Pressed)
            {
                if (!_leftPressed)
                {
                    _leftPressed = true;
                    EntityManager.Add(new egg(_placeCube.Position, new Quaternion(0, 0, 0, 0), _cube, _wood));
                }
            }
            else _leftPressed = false;

            if (mState.RightButton == ButtonState.Pressed)
                EntityManager.Add(new egg(_placeCube.Position, new Quaternion(0, 0, 0, 0), _cube, _wood));

            EntityManager.Update(gameTime, kState, mState);

            Simulation?.Timestep(1 / 60f);

            LastMouseState = mState;
            if (kState.IsKeyDown(Keys.Escape))
                _escapeHeld = true;

            if (kState.IsKeyUp(Keys.Escape) && _escapeHeld)
                SE2.LoadScene(new MenuScene(SE2));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix viewMatrix = Camera.ViewMatrix;
            Matrix projectionMatrix = Camera.ProjectionMatrix;
            
            _placeCube.Draw(_world, viewMatrix, projectionMatrix);
            EntityManager.Draw(_world, viewMatrix, projectionMatrix);
        }
    }
}