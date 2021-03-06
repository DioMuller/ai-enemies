using System;
using Enemies.Auxiliary;
using Enemies.Entities;
using Enemies.GUI;
using Jv.Games.Xna.Async;
using Jv.Games.Xna.Context;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Enemies.Screens
{
    class TitleScreen : Screen<TitleScreen.Result>, INotifyPropertyChanged
    {
        #region Nested
        public enum Result
        {
            Exit,
            StartSandbox,
            StartGame,
            LoadGame
        }
        #endregion

        #region Attributes
        Task _preloadGameContent;
        Rectangle _positionRect;
        public readonly UIEntity GUI;
        public readonly Cursor Cursor;
	    private Texture2D background;
        float _titleAngleY, _titleAngleX;
        #endregion

        #region Properties
        public float TitleAngleY
        {
            get { return _titleAngleY; }
            set
            {
                _titleAngleY = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TitleAngleY"));
            }
        }
        public float TitleAngleX
        {
            get { return _titleAngleX; }
            set
            {
                _titleAngleX = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TitleAngleX"));
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors
        public TitleScreen(MainGame game)
            : base(game)
        {
            var title = new ScreenTitleMain { BindingContext = this };

            title.OnNewGame += new OnButtonClickDelegate(() =>
            {
                Exit(Result.StartGame);
            });

            title.OnSandbox += new OnButtonClickDelegate(() =>
            {
                Exit(Result.StartSandbox);
            });

            title.OnLoadGame += new OnButtonClickDelegate(() =>
            {
                Exit(Result.LoadGame);
            });

            title.OnQuitGame += new OnButtonClickDelegate(() =>
            {
                Exit(Result.Exit);
            });

            GUI = title.AsEntity();
            GUI.Size = new Xamarin.Forms.Size(Viewport.Width, Viewport.Height);

	        background = Content.Load<Texture2D>("GUI/title_background");
            Cursor = new Cursor(Content);

			AudioPlayer.PlayBGM("Exhilarate");
        }
        #endregion

        #region Life Cycle
        protected override void LoadContent()
        {
            // TODO: Load content here! (via base.Content)
            base.LoadContent();
        }

        protected async override Task InitializeScreen()
        {
            _preloadGameContent = GameScreen.PreloadContent(Content);

            FadeColor = Color.Black;

            _positionRect = new Rectangle(0, 0, Viewport.Width, Viewport.Height);

            await UpdateContext.CompleteWhen(CanStart);
            await FadeIn();
        }

        bool CanStart(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            return !keyboard.IsKeyDown(Keys.Back) && !keyboard.IsKeyDown(Keys.Enter);
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
            GUI.Update(gameTime);

            if (State != RunState.Running)
                return;

            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Escape))
                Exit(Result.Exit);
            if (keyboard.IsKeyDown(Keys.Enter))
                Exit(Result.StartGame);

            var mouse = Mouse.GetState();
            TitleAngleY = Math.Min( Math.Max( (mouse.X / (float)Viewport.Width - 0.5f) * 45, -10), 10);
            TitleAngleX = Math.Min(Math.Max( 1 - (mouse.Y / (float)Viewport.Height) * 45, -10), 10);

            Cursor.Update(gameTime);
        }

        #endregion

        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

			// TODO: Didn't do that on WPF, tried but was a pain to order the image.
			SpriteBatch.Begin();
			SpriteBatch.Draw(background, new Rectangle(0, 0, 800, 600), null, Color.White);
			SpriteBatch.End();

            GUI.Draw(SpriteBatch, gameTime);

            Cursor.Draw(SpriteBatch, gameTime);
        }

        #endregion

        #endregion
    }
}

