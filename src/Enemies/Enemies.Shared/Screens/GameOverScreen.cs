using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enemies.Screens
{
    class GameOverScreen : Screen<GameOverScreen.Result>
    {
        #region Nested
        public enum Result
        {
            Over
        }
        #endregion Nested

        #region Attributes
        private static SpriteFont _titleFont = null;
        private static Vector2 _center;
        private static Vector2 _descriptionPosition;
        private static string _text;
        private static Color _titleColor;
        private Vector2 _titleSize;
        private Vector2 _textSize;
        #endregion Attributes

        #region Constructor
        public GameOverScreen(MainGame game, string text, Color color)
            : base(game)
        {
            _text = text;
            _titleColor = color;

            _titleFont = Content.Load<SpriteFont>("Fonts/DefaultFont");
            _center = new Vector2(Game.Window.ClientBounds.Center.X, Game.Window.ClientBounds.Center.Y);
            _descriptionPosition = _center + new Vector2(0.0f, 200.0f);

            _textSize = _titleFont.MeasureString("Press Enter to go to the Title Screen");
            _titleSize = _titleFont.MeasureString(text);
        }
        #endregion Constructor

        #region Methods
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(_titleFont, _text, _center, _titleColor, 0.0f, 
                _titleSize / 2.0f, new Vector2(3.0f, 3.0f), SpriteEffects.None, 1.0f);

            SpriteBatch.DrawString(_titleFont, "Press Enter to go to the Title Screen", _descriptionPosition, Color.White, 0.0f,
                _textSize / 2.0f, new Vector2(1.0f, 1.0f), SpriteEffects.None, 1.0f);
            SpriteBatch.End();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if( Keyboard.GetState().IsKeyDown(Keys.Enter) )
            {
                Exit(Result.Over);
            }
        }
        #endregion Methods
    }
}
