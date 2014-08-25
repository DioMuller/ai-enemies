using Enemies.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Immutable;

namespace Enemies.Screens
{
    class GameScreen : Screen<GameScreen.Result>
    {
        #region Nested
        public enum Result
        {
            ReturnToTitle
        }
        #endregion

        #region Properties
        public ImmutableList<IEntity> Entities = ImmutableList<IEntity>.Empty;
        #endregion

        #region Constructors
        public GameScreen(MainGame game)
            : base(game)
        {
        }
        #endregion

        #region Life Cycle
        protected override void LoadContent()
        {
            var p = new PythonEntityFactory();

            Entities = Entities.Add(p.LoadEntity(Content, "main"));

            // TODO: Load game content!
            base.LoadContent();
        }
        #endregion

        #region Game Loop

        #region Update
        protected override void Update(GameTime gameTime)
        {
            if (CheckFinish(gameTime))
                return;

            foreach (var entity in Entities)
                entity.Update(gameTime);

            base.Update(gameTime);
        }

        bool CheckFinish(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape))
            {
                Exit(Result.ReturnToTitle);
                return true;
            }

            return false;
        }

        #endregion

        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Yellow);

            SpriteBatch.Begin();

            try
            {
                foreach (var entity in Entities)
                    entity.Draw(SpriteBatch, gameTime);
            }
            finally
            {
                SpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        #endregion

        #endregion
    }
}
