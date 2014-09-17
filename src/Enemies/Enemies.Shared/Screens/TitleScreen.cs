using Jv.Games.Xna.Async;
using Jv.Games.Xna.Context;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;

namespace Enemies.Screens
{
    class TitleScreen : Screen<TitleScreen.Result>
    {
        #region Nested
        public enum Result
        {
            Exit,
            StartGame
        }
        #endregion

        #region Attributes
        Task _preloadGameContent;
        Texture2D _titleTexture;
        Rectangle _positionRect;
        #endregion

        #region Constructors
        public TitleScreen(MainGame game)
            : base(game)
        {
        }
        #endregion

        #region Life Cycle
        protected override void LoadContent()
        {
            // TODO: Load content here! (via base.Content)
            base.LoadContent();

            _titleTexture = Content.Load<Texture2D>("GUI/title");
        }

        protected async override Task InitializeScreen()
        {
            _preloadGameContent = GameScreen.PreloadContent(Content);

            FadeColor = Color.Black;

            _positionRect = new Rectangle(0, 0, 800, 600);

            await UpdateContext.CompleteWhen(CanStart);
            await FadeIn();
        }

        bool CanStart(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            return !keyboard.IsKeyDown(Keys.Escape) && !keyboard.IsKeyDown(Keys.Enter);
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
            if (State != RunState.Running)
                return;

            // TODO: Screen update logic!
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape))
                Exit(Result.Exit);
            if (keyboard.IsKeyDown(Keys.Enter))
                Exit(Result.StartGame);
        }

        #endregion

        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();
            SpriteBatch.Draw(_titleTexture, _positionRect, null, Color.White);
            SpriteBatch.End();
        }

        #endregion

        #endregion
    }
}

