using Jv.Games.Xna.Context;
using Jv.Games.Xna.Async;
using Jv.Games.Xna.Base;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Enemies.Screens
{
    class Screen<TResult> : Activity<TResult>
    {
        #region Nested
        public enum RunState
        {
            NotRunning,
            Initializing,
            Running,
            Finalizing,
            Completed
        }
        #endregion

        #region Attributes

        bool _contentInitialized;
        protected RunState State;
        protected Reference<Color> FadeColor = Color.Transparent;
        Texture2D _fadeTexture;
        #endregion

        #region Properties

        new public MainGame Game { get { return (MainGame)base.Game; } }

        #endregion

        #region Constructors

        public Screen(MainGame game)
            : base(game)
        {
        }

        #endregion

        #region Life Cycle

        protected override void Initialize()
        {
            if (!_contentInitialized)
            {
                _contentInitialized = true;
                _fadeTexture = new Texture2D(GraphicsDevice, 1, 1);
                _fadeTexture.SetData(new[] { Color.White });
                LoadContent();
            }
        }

        protected virtual void LoadContent()
        {
        }

        /// <summary>
        /// Represents the activity execution.
        /// When the screen exit, the returning task will
        /// complete with the result.
        /// </summary>
        /// <returns>The activity task.</returns>
        protected async override Task<TResult> Run()
        {
            State = RunState.Initializing;

            var initializeTask = InitializeScreen();
            if (initializeTask != null)
                await UpdateContext.Wait(initializeTask);

            State = RunState.Running;

            var result = await UpdateContext.Wait(base.Run());

            State = RunState.Finalizing;

            var finalizeTask = FinalizeScreen();
            if (finalizeTask != null)
                await UpdateContext.Wait(finalizeTask);

            State = RunState.Completed;

            return result;
        }

        /// <summary>
        /// Optional async operation to run when the screen is played.
        /// During this operation the screen update / draw will be called.
        /// </summary>
        /// <returns>The screen initialization task.</returns>
        protected virtual Task InitializeScreen()
        {
            return null;
        }

        /// <summary>
        /// Optional async operation to run when the screen exits.
        /// During this operation the screen update / draw will be called.
        /// </summary>
        /// <returns>The screen completion task.</returns>
        protected virtual Task FinalizeScreen()
        {
            return null;
        }

        #endregion

        #region Game Loop

        protected override void DrawActivity(GameTime gameTime)
        {
            try
            {
                base.DrawActivity(gameTime);
                if (FadeColor != Color.Transparent)
                {
                    SpriteBatch.Begin();
                    SpriteBatch.Draw(_fadeTexture, new Rectangle(0, 0, Viewport.Width, Viewport.Height), FadeColor.Value);
                    SpriteBatch.End();
                }
            }
            catch (Exception ex)
            {
                ActivityCompletion.TrySetException(ex);
            }
        }

        protected override void UpdateActivity(GameTime gameTime)
        {
            try
            {
                base.UpdateActivity(gameTime);
            }
            catch (Exception ex)
            {
                ActivityCompletion.TrySetException(ex);
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Post the specified method to be executed
        /// during the next update loop.
        /// </summary>
        /// <param name="asyncMethod">Async method.</param>
        public void Post(Action<Screen<TResult>> asyncMethod)
        {
            UpdateContext.Post(gameTime => asyncMethod(this));
        }

        #endregion

        #region Protected Methods
        public ContextOperation<TimeSpan> FadeIn(TimeSpan? duration = null)
        {
            duration = duration ?? TimeSpan.FromSeconds(0.5);
            return DrawContext.Animate(duration.Value, FadeColor, Color.Transparent);
        }

        public ContextOperation<TimeSpan> FadeOut(TimeSpan? duration = null)
        {
            duration = duration ?? TimeSpan.FromSeconds(0.5);
            return DrawContext.Animate(duration.Value, FadeColor, Color.Black);
        }
        #endregion
    }
}

