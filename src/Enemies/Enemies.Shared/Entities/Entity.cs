﻿using Jv.Games.Xna.Async;
using Microsoft.Xna.Framework;

namespace Enemies.Entities
{
    interface IEntity
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
    }

    class Entity : IEntity
    {
        #region Attributes
        protected readonly AsyncContext UpdateContext, DrawContext;
        #endregion

        #region Constructors
        public Entity()
        {
            UpdateContext = new AsyncContext();
            DrawContext = new AsyncContext();
        }
        #endregion

        #region IEntity
        void IEntity.Update(GameTime gameTime)
        {
            Update(gameTime);
            UpdateContext.Update(gameTime);
        }

        void IEntity.Draw(GameTime gameTime)
        {
            Draw(gameTime);
            DrawContext.Update(gameTime);
        }
        #endregion

        #region Game Loop
        protected virtual void Update(GameTime gameTime)
        {

        }

        protected virtual void Draw(GameTime gameTime)
        {

        }
        #endregion
    }
}
