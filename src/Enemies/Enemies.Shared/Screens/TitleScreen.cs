using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Enemies.Screens
{
    public class TitleScreen : Screen<TitleScreen.Result>
    {
        public enum Result
        {
            Exit
        }

        public TitleScreen(MainGame game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            // TODO: Load content here! (via base.Content)
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Screen update logic!
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape))
                Exit(Result.Exit);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Screen rendering (via base.SpriteBatch)
            GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}

