using Enemies.Entities;
using Enemies.Scripting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Jv.Games.Xna.Async;
using System;
using System.Linq;
using Enemies.Parameters;

namespace Enemies.Screens
{
    class GameScreen : Screen<GameScreen.Result>
    {
        #region Const
        const string Tag = "GameScreen";

        private const float ParameterUpdateTime = 100.0f;
        #endregion Const

        #region Static

        static float _parameterUpdateCounter = ParameterUpdateTime;

        static ScriptEntityFactory _scriptEntityFactory;

        static Task _contentLoad;

        public static Task PreloadContent(ContentManager content)
        {
            if (_contentLoad != null)
                return _contentLoad;

            return _contentLoad = Task.Factory.StartNew(delegate
            {
                _scriptEntityFactory = new ScriptEntityFactory(content);

                var preloadEntities = _scriptEntityFactory.AvailableEntities(content)
                        .Select(s =>
                        {
                            try
                            {
                                return _scriptEntityFactory.LoadEntity(content, s, new Vector2(0,0));
                            }
                            catch (Exception ex)
                            {
                                Log.Error(Tag, ex);
                                return null;
                            }
                        })
                        .ToList();
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
            var availableEntities = _scriptEntityFactory.AvailableEntities(Content).ToList();

            //if (availableEntities.Count > 0)
            //foreach (var entity in availableEntities)
            //    Entities = Entities.Add(_scriptEntityFactory.LoadEntity(Content, entity));//availableEntities[0]));
            await FadeIn();
        }

        protected override async Task FinalizeScreen()
        {
            await FadeOut();
        }

        #endregion

        #region Exposed Methods

        public bool AddEntity(string entity, Vector2 position)
        {
            if (!_scriptEntityFactory.AvailableEntities(Content).Contains(entity)) return false;

            Entities = Entities.Add(_scriptEntityFactory.LoadEntity(Content, entity, position));
            return true;
        }
        #endregion Exposed Methods

        #region Game Loop

        #region Update

        protected override void Update(GameTime gameTime)
        {
            if (State == RunState.Initializing)
                return;

            if (CheckFinish(gameTime))
            {
                _parameterUpdateCounter = 0.0f;
                return;
            }

            #region Parameter Update
            _parameterUpdateCounter += gameTime.ElapsedGameTime.Milliseconds;
            if (_parameterUpdateCounter >= ParameterUpdateTime)
            {
                _parameterUpdateCounter -= ParameterUpdateTime;

                GameParameters.UpdateEntities(Entities);
            }
            #endregion Parameter Update

            foreach (var entity in Entities)
                TryUpdateEntity(gameTime, entity);

            base.Update(gameTime);
        }

        void TryUpdateEntity(GameTime gameTime, IEntity entity)
        {
            try
            {
                entity.Update(gameTime);
            }
            catch (Exception ex)
            {
                Log.Error(Tag, ex);
                Entities = Entities.Remove(entity);
            }
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
            foreach (var entity in Entities)
                TryDrawEntity(gameTime, entity);
            SpriteBatch.End();
        }

        void TryDrawEntity(GameTime gameTime, IEntity entity)
        {
            try
            {
                entity.Draw(SpriteBatch, gameTime);
            }
            catch (Exception ex)
            {
                Log.Error(Tag, ex);
                Entities = Entities.Remove(entity);
            }
        }

        #endregion

        #endregion
    }
}
