using Enemies.Entities;
using Enemies.GUI;
using Enemies.Maps;
using Enemies.Parameters;
using Enemies.Scripting;
using Jv.Games.Xna.Async;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

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
                                return _scriptEntityFactory.LoadEntity(content, s, new Vector2(0, 0));
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

        #region Attributes
        public readonly UIEntity GUI;
        public readonly Cursor Cursor;

        private bool _guiVisible = true;
        private bool _isPaused = false;
        private TypeTag _currentTag = TypeTag.Enemy;
        private ScriptEntityDescription _currentEntity = null;

        private bool _tabPressed = false;
        private bool _mousePressed = false;
        #endregion

        #region Properties

        public ImmutableList<IEntity> Entities = ImmutableList<IEntity>.Empty;

        #endregion

        #region Constructors

        public GameScreen(MainGame game)
            : base(game)
        {
            GUI = CreateGUI().AsEntity();
            GUI.Size = new Xamarin.Forms.Size(Viewport.Width, Viewport.Height);

            Cursor = new Cursor(Content);
        }

        #endregion

        #region GUI
        Xamarin.Forms.Page CreateGUI()
        {
            var mainMenu = new ScreenGameMain();
            var entities = _scriptEntityFactory.AvailableEntities(Content).ToImmutableList();

            mainMenu.AddEntity_Page += () =>
            {
                return CreateEntitySelectionGUI();
            };

            mainMenu.BuildMode_Clicked += () =>
            {
                Cursor.CurrentState = CursorState.Build;
            };

            mainMenu.PlayPause_Clicked += () =>
            {
                _isPaused = !_isPaused;
            };

            return new Xamarin.Forms.NavigationPage(mainMenu);
        }

        Xamarin.Forms.Page CreateEntitySelectionGUI()
        {
            var entityMenu = new ScreenEntityTag();
            var entities = _scriptEntityFactory.AvailableEntities(Content).ToImmutableList();

            Action<string> addEntity = async category =>
            {
                _mousePressed = true;
                _currentTag = GetTag(category);

                var selectionScreen = await UpdateContext.Wait(Task.Factory.StartNew(() => new ScreenEntitySelection { Items = entities }));
                await entityMenu.Navigation.PushAsync(selectionScreen);
                var selectedScript = await selectionScreen.SelectItemAsync();

                PlaceEntity(selectedScript);

                await selectionScreen.Navigation.PopAsync();
            };

            entityMenu.AddPlayer_Clicked += () => addEntity("Player");
            entityMenu.AddEnemy_Clicked += () => addEntity("Enemy");
            entityMenu.AddObjective_Clicked += () => addEntity("Objective");
            return new Xamarin.Forms.NavigationPage(entityMenu);
        }
        #endregion GUI

        #region Life Cycle
        protected override async Task InitializeScreen()
        {
            FadeColor = Color.Black;
            await PreloadContent(Content);
            var availableEntities = _scriptEntityFactory.AvailableEntities(Content).ToList();

            /*
            if (availableEntities.Count > 0)
            foreach (var entity in availableEntities)
                Entities = Entities.Add(_scriptEntityFactory.LoadEntity(Content, entity, new Vector2(0,0)));//availableEntities[0]));
             */

            // TODO: non-fixed map loading.
            GameParameters.LoadMap(this, new Point(800, 600), Content, "Maps/map01");

            await FadeIn();
        }

        protected override async Task FinalizeScreen()
        {
            await FadeOut();
        }

        #endregion

        #region Exposed Methods

        void PlaceEntity(ScriptEntityDescription entity)
        {
            Log.Debug(Tag, "Item Selected: " + entity.DisplayName);

            _mousePressed = true;
            _currentEntity = entity;
            Cursor.CurrentState = CursorState.AddEntity;
        }

        public TypeTag GetTag(string tag)
        {
            if(tag == "Player") return TypeTag.Player;
            if(tag == "Enemy") return TypeTag.Enemy;
            if(tag == "Objective") return TypeTag.Objective;
            return TypeTag.None;
        }

        public bool AddEntity(string entityFile, Vector2 position, TypeTag tag)
        {
            var entity = _scriptEntityFactory
                .AvailableEntities(Content)
                .FirstOrDefault(e => System.IO.Path.GetFileName(e.File) == entityFile);

            if (entity == null)
                return false;

            return AddEntity(entity, position, tag);
        }

        public bool AddEntity(ScriptEntityDescription entity, Vector2 position, TypeTag tag)
        {
            var e = _scriptEntityFactory.LoadEntity(Content, entity, position);
            e.Tag = tag;
            Entities = Entities.Add(e);
            return true;
        }
        #endregion Exposed Methods

        #region Game Loop

        #region Update

        protected override void Update(GameTime gameTime)
        {
            if (_guiVisible) GUI.Update(gameTime);
            Cursor.Update(gameTime);

            #region Cursor Actions
            MouseState mouse = Mouse.GetState();

            switch (Cursor.CurrentState)
            {
                case CursorState.Build:
                    if (mouse.LeftButton == ButtonState.Pressed || mouse.RightButton == ButtonState.Pressed)
                    {
                        var quad = GameParameters.CurrentMap.GetQuadrant(Cursor.Position);
                        GameParameters.CurrentMap.ChangeQuadrant(quad, mouse.LeftButton == ButtonState.Pressed ? Tile.Wall : Tile.Ground);
                    }
                    break;
                case CursorState.AddEntity:
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        Vector2 position = new Vector2(Cursor.Position.X, Cursor.Position.Y);
                        if (_currentEntity != null && _currentTag != TypeTag.None 
                            && !GameParameters.CurrentMap.CollidesWithMap(Cursor.CollisionRect) 
                            && !_mousePressed)
                        {
                            AddEntity(_currentEntity, position, _currentTag);
                            _mousePressed = true;
                        }
                    }
                    else if (mouse.RightButton == ButtonState.Pressed)
                    {
                        _mousePressed = true;
                    }
                    else
                    {
                        _mousePressed = false;
                    }
                    break;
                default:
                    break;
            }
            #endregion Cursor Actions

            #region Keyboard Shortcuts

            KeyboardState keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.Tab))
            {
                if (!_tabPressed)
                {
                    _guiVisible = !_guiVisible;
                    _tabPressed = true;
                }
            }
            else
            {
                _tabPressed = false;
            }
            #endregion Keyboard Shortcuts

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
            if (keyboard.IsKeyDown(Keys.Back))
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();

            if (GameParameters.CurrentMap != null) GameParameters.CurrentMap.Draw(SpriteBatch, gameTime);
            foreach (var entity in Entities)
                TryDrawEntity(gameTime, entity);
            SpriteBatch.End();

            if (_guiVisible) GUI.Draw(SpriteBatch, gameTime);
            Cursor.Draw(SpriteBatch, gameTime);
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
