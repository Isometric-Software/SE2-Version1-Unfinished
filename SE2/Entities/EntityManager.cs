using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SE2.Entities
{
    public static class EntityManager
    {
        private static List<Entity> _entities = new List<Entity>();
        private static List<Entity> _entitiesToAdd = new List<Entity>();
        private static List<Entity> _entitiesToRemove = new List<Entity>();

        private static bool _isUpdating;

        public static int EntityCount => _entities.Count;
        
        public static void Update(GameTime gameTime, KeyboardState kState, MouseState mState)
        {
            _isUpdating = true;
            foreach (Entity entity in _entities)
                entity.Update(gameTime, kState, mState);
            _isUpdating = false;
            
            foreach(Entity entity in _entitiesToAdd)
                _entities.Add(entity);
            _entitiesToAdd.Clear();

            foreach (Entity entity in _entitiesToRemove)
                _entities.Remove(entity);
            _entitiesToRemove.Clear();
        }

        public static void Draw(Matrix world, Matrix view, Matrix projection)
        {
            foreach (Entity entity in _entities)
                entity.Draw(world, view, projection);
        }

        public static void Add(Entity entity)
        {
            if (_isUpdating)
                _entitiesToAdd.Add(entity);
            else
                _entities.Add(entity);
        }

        public static void Clear()
        {
            _entities.Clear();
        }

        public static void Remove(Entity entity)
        {
            if (_isUpdating)
                _entitiesToRemove.Add(entity);
            else
                _entities.Remove(entity);
        }
    }
}