using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SE2.Entities
{
    public abstract class Entity
    {
        public Model Model { get; }
        public Texture2D Texture { get; set; }
        
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
        public bool Show;

        public Entity(Vector3 position, Quaternion rotation, Model model, Texture2D texture = null, Vector3 scale = default, bool show = true)
        {
            Model = model;
            Position = position;
            Rotation = rotation;
            Scale = scale == default ? new Vector3(1) : scale;
            Show = show;
            Texture = texture;
        }
        
        public abstract void Update(GameTime gameTime, KeyboardState kState, MouseState mState);

        public virtual void Draw(Matrix world, Matrix view, Matrix projection)
        {
            if (!Show) return;
            
            world *= Matrix.CreateFromQuaternion(Rotation) * Matrix.CreateScale(Scale) *
                     Matrix.CreateTranslation(Position);

            foreach (ModelMesh mesh in Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = world;
                    effect.View = view;
                    effect.Projection = projection;
                    effect.EnableDefaultLighting();
                    if (Texture == null) continue;
                    effect.Texture = Texture;
                    effect.TextureEnabled = true;
                }
                mesh.Draw();
            }
        }
    }
}