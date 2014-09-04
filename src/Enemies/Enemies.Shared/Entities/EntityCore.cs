using Enemies.Behaviors;
using Jv.Games.Xna.Context;
using Jv.Games.Xna.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Immutable;

namespace Enemies.Entities
{
    public interface IEntity
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }

    internal class EntityCore : IEntity
    {
        #region Attributes
        public readonly Context UpdateContext, DrawContext;
        public readonly Sprite Sprite;
        public IImmutableList<IBehavior> Behaviors = ImmutableList<IBehavior>.Empty;
        #endregion

        #region Constructors
        public EntityCore()
        {
            UpdateContext = new Context();
            DrawContext = new Context();
            Sprite = new Sprite();
        }
        #endregion

        #region IEntity
        void IEntity.Update(GameTime gameTime)
        {
            try
            {
                Update(gameTime);
            }
            finally
            {
                UpdateContext.Update(gameTime);
            }
        }

        void IEntity.Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            try
            {
                Draw(spriteBatch, gameTime);
            }
            finally
            {
                DrawContext.Update(gameTime);
            }
        }
        #endregion

        #region Game Loop
        protected virtual void Update(GameTime gameTime)
        {
            foreach(var behavior in Behaviors)
            {
                if (behavior.Enabled)
                    behavior.Update(gameTime);
            }
        }

        protected virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Update(gameTime);
            Sprite.Draw(spriteBatch, gameTime);
        }
        #endregion
    }
}
