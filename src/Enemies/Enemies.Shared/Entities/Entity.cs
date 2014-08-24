using Jv.Games.Xna.Async;
using Jv.Games.Xna.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Enemies.Entities
{
    interface IEntity
    {
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }

    class Entity : IEntity
    {
        #region Attributes
        public readonly AsyncContext UpdateContext, DrawContext;
        public readonly Sprite Sprite;
        #endregion

        #region Constructors
        public Entity()
        {
            UpdateContext = new AsyncContext();
            DrawContext = new AsyncContext();
            Sprite = new Sprite();
        }
        #endregion

        #region IEntity
        void IEntity.Update(GameTime gameTime)
        {
            Update(gameTime);
            UpdateContext.Update(gameTime);
        }

        void IEntity.Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Draw(spriteBatch, gameTime);
            DrawContext.Update(gameTime);
        }
        #endregion

        #region Game Loop
        protected virtual void Update(GameTime gameTime)
        {
        }

        protected virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Sprite.Update(gameTime);
            Sprite.Draw(spriteBatch, gameTime);
        }
        #endregion
    }
}
