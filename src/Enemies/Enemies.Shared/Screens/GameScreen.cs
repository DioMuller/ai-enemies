using Enemies.Entities;
using Enemies.Scripting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Jv.Games.Xna.Async;

namespace Enemies.Screens
{
    class GameScreen : Screen<GameScreen.Result>
    {
        #region Static
        static ScriptEntityFactory _scriptEntityFactory;

        static Task _contentLoad;
        public static Task PreloadContent(ContentManager content)
        {
            if (_contentLoad != null)
                return _contentLoad;

            return _contentLoad = Task.Factory.StartNew(delegate
            {
                _scriptEntityFactory = new ScriptEntityFactory();
                _scriptEntityFactory.LoadEntity(content, "main");
            });
        }
        #endregion

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
        protected override async Task InitializeScreen()
        {
            FadeColor = Color.Black;
            await PreloadContent(Content);
            Entities = Entities.Add(_scriptEntityFactory.LoadEntity(Content, "main"));
            await FadeIn();
        }

        protected override async Task FinalizeScreen()
        {
            await FadeOut();
        }

        #endregion

        #region Game Loop

        #region Update
        protected override void Update(GameTime gameTime)
        {
            if (State == RunState.Initializing)
                return;

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

            //base.Draw(gameTime);
        }

        #endregion

        #endregion
    }
}
