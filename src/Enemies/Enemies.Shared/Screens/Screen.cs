using Jv.Games.Xna.Async;
using System.Threading.Tasks;
using System;

namespace Enemies.Screens
{
    public class Screen<TResult> : Activity<TResult>
    {
        #region Attributes

        bool _initialized;

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

        #region Life-Cycle

        protected override void Initialize()
        {
            if (!_initialized)
            {
                _initialized = true;
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
        protected async override Task<TResult> RunActivity()
        {
            var initializeTask = InitializeScreen();
            if (initializeTask != null)
                await initializeTask;

            var result = await base.RunActivity();

            var finalizeTask = FinalizeScreen();
            if (finalizeTask != null)
                await finalizeTask;

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
    }
}

