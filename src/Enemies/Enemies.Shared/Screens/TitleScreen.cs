using Enemies.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

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
        }

        protected async override System.Threading.Tasks.Task InitializeScreen()
        {
            await UpdateContext.CompleteWhen(gt => !Keyboard.GetState().IsKeyDown(Keys.Escape));
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
            // TODO: Screen rendering (via base.SpriteBatch)
            GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        #endregion

        #endregion
    }
}

