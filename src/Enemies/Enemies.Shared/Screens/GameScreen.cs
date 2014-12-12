using Enemies.Auxiliary;
using Enemies.Entities;
using Enemies.GUI;
using Enemies.Maps;
using Enemies.Parameters;
using Enemies.Scripting;
using Jv.Games.Xna.Async;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
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

        protected static Stack<BaseEntity> _toAdd = new Stack<BaseEntity>();

        protected static GameScreen current = null;

        protected static void AddEntity(BaseEntity entity)
        {
            _toAdd.Push(entity);
        }

        public static void CreateBullet(TypeTag targetTag, Vector2 position, Vector2 direction, float velocity)
        {
            direction.Normalize();
            current.CreateBulletInstance(targetTag, position, direction, velocity);
        }

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
            ReturnToTitle,
            LoadNext,
            GameOver,
            Win
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
		private bool _sandbox = true;

        List<Vector2> _playerPositions = new List<Vector2>();
        Stack<BaseEntity> _toRemove = new Stack<BaseEntity>();

        private SpriteFont _font;
        private Vector2 _textPosition;
        private string _text;
        private Vector2 _origin;

		#region Hack

	    private OnButtonClickDelegate _entityClick = null;
	    private OnButtonClickDelegate _playerClick = null;
        private OnButtonClickDelegate _mapClick = null;
		#endregion Hack

        private string _map = "";
        #endregion

        #region Properties

        public ImmutableList<IEntity> Entities = ImmutableList<IEntity>.Empty;

        #endregion

        #region Constructors

        public GameScreen(MainGame game, bool sandbox = true, string map = "new", bool loadmap = false)
            : base(game)
        {
	        Cursor = new Cursor(Content);

            current = this;

	        _sandbox = sandbox;
            _map = "Maps/" + map;

			GUI = CreateGUI().AsEntity();

			// HACK
	        if (!_sandbox)
	        {
		        _entityClick();
		        _playerClick();
	        }
            else if (loadmap)
            {
                _mapClick();
            }

			GUI.Size = new Xamarin.Forms.Size(Viewport.Width, Viewport.Height);

			//Load Music
	        if (_sandbox)
	        {
		        AudioPlayer.PlayBGM("Wah Game Loop");
	        }
	        else
	        {
		        AudioPlayer.PlayBGM("Take the Lead");
	        }

            _font = Content.Load<SpriteFont>("Fonts/DefaultFont");
            _textPosition = new Vector2(400, 20);
            _text = "Press TAB to go back";
            _origin = _font.MeasureString(_text) / 2;

	        MessageManager.ClearMessages();
        }

        #endregion

        #region GUI
        Xamarin.Forms.Page CreateGUI()
        {
            var mainMenu = new ScreenGameMain();
            var entities = _scriptEntityFactory.AvailableEntities(Content).ToImmutableList();

            mainMenu.AddEntity_Page += () =>
            {
                Cursor.CurrentState = CursorState.Normal;
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

            
            mainMenu.LoadMap_Clicked += () =>
            {
                var loadMap = GetLoadMapAction(mainMenu);

                loadMap();
            };

            // HACK 2
            _mapClick += () =>
            {
                var loadMap = GetLoadMapAction(mainMenu);

                loadMap();
            };

			// HACK
			if( !_sandbox )
		        _entityClick += () => mainMenu.AddEntity_Click(null, null);

            return new Xamarin.Forms.NavigationPage(mainMenu);
        }

        Xamarin.Forms.Page CreateEntitySelectionGUI()
        {
            var entityMenu = new ScreenEntityTag(_sandbox);
            var entities = _scriptEntityFactory.AvailableEntities(Content).OrderBy(e => e.DisplayName).ToImmutableList();

            Action<string> addEntity = async category =>
            {
                _mousePressed = true;
                _currentTag = GetTag(category);
                Cursor.CurrentState = CursorState.Normal;

                var selectionScreen = await UpdateContext.Wait(Task.Factory.StartNew(() => new ScreenEntitySelection { Items = entities }));
                await entityMenu.Navigation.PushAsync(selectionScreen);
                var selectedScript = await selectionScreen.SelectItemAsync();

                _guiVisible = false;
                PlaceEntity(selectedScript);

                await selectionScreen.Navigation.PopAsync();
            };

            entityMenu.AddPlayer_Clicked += () => addEntity("Player");
            entityMenu.AddEnemy_Clicked += () => addEntity("Enemy");
            entityMenu.AddObjective_Clicked += () => addEntity("Objective");

			// HACK
			if (!_sandbox)
				_playerClick += () => addEntity("Player");

            return entityMenu;
        }

        // Map Selection
        Action GetLoadMapAction(ScreenGameMain mainMenu)
        {
            var engine = new MapEngine(Content);
            var maps = engine.AvailableEntities(Content).OrderBy(e => e.DisplayName).ToImmutableList();
            

            Action loadMap = async () =>
            {
                _mousePressed = true;
                Cursor.CurrentState = CursorState.Normal;

                var selectionScreen = await UpdateContext.Wait(Task.Factory.StartNew(() => new ScreenEntitySelection { Items = maps }));
                await mainMenu.Navigation.PushAsync(selectionScreen);
                var map = await selectionScreen.SelectItemAsync();

                _guiVisible = false;

                GameParameters.SetNextMap(map.DisplayName);
                Exit(Result.LoadNext);
            };

            return loadMap;
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

            GameParameters.LoadMap(this, new Point(800, 600), Content, _map);

            if( !_sandbox )
            {
                _isPaused = true;

                foreach(var entity in Entities.OfType<BaseEntity>())
                {
                    if( entity.Tag == TypeTag.Player )
                    {
                        _playerPositions.Add(entity.Position);
                        _toRemove.Push(entity);
                    }
                }

                CleanEntities();
            }

            await FadeIn();
        }

        protected override async Task FinalizeScreen()
        {
            await FadeOut();
        }

        protected void CleanEntities()
        {
            while (_toRemove.Count > 0)
            {
                Entities = Entities.Remove(_toRemove.Pop());
            }

            while (_toAdd.Count > 0)
            {
                Entities = Entities.Add(_toAdd.Pop());
            }
        }

        #endregion

        #region Exposed Methods

        void PlaceEntity(ScriptEntityDescription entity)
        {
			Log.Debug(Tag, "Item Selected: " + entity.DisplayName);

	        if (!_sandbox)
	        {
		        foreach (var location in _playerPositions)
		        {
			        AddEntity(entity, location, TypeTag.Player);
		        }

		        _isPaused = false;
	        }
	        else
	        {
				_mousePressed = true;
				_currentEntity = entity;
				Cursor.CurrentState = CursorState.AddEntity;
	        }
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

        public void CreateBulletInstance(TypeTag targetTag, Vector2 position, Vector2 direction, float velocity)
        {
            Entities = Entities.Add(new BulletEntity(Content, targetTag, position, direction, velocity));
        }
        #endregion Exposed Methods

        #region Game Loop

        #region Update

        protected override void Update(GameTime gameTime)
        {
			#region GUI
            if (_guiVisible) GUI.Update(gameTime);
            Cursor.Update(gameTime);
			#endregion GUI

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
                if (!_tabPressed && _sandbox)
                {
                    _guiVisible = !_guiVisible;
                    Cursor.CurrentState = CursorState.Normal;
                    _tabPressed = true;
                }
            }
            else
            {
                _tabPressed = false;
            }

            if(keys.IsKeyDown(Keys.Escape))
            {
                Exit(Result.ReturnToTitle);
            }
            #endregion Keyboard Shortcuts

			#region Lifetime
            if (State == RunState.Initializing)
                return;

            if (CheckFinish(gameTime))
            {
                _parameterUpdateCounter = 0.0f;
                return;
            }
			#endregion Lifetime

            #region Parameter Update
            _parameterUpdateCounter += gameTime.ElapsedGameTime.Milliseconds;
            if (_parameterUpdateCounter >= ParameterUpdateTime)
            {
                _parameterUpdateCounter -= ParameterUpdateTime;

                GameParameters.UpdateEntities(Entities);
            }
            #endregion Parameter Update

            #region Messages
            MessageManager.ProcessMessages((message) =>
                {
                    if( message.Receiver == -1)
                    {
                        foreach (var entity in Entities.OfType<BaseEntity>())
                        {
                            if( entity.Id != message.Sender ) entity.ReceiveMessage(message);
                        }
                    }
                    else
                    {
                        var entity = Entities.OfType<BaseEntity>().FirstOrDefault(e => message.Receiver == e.Id);

                        if( entity != null )
                        {
                            entity.ReceiveMessage(message);
                        }
                    }
                });
            #endregion Messages

			#region Entity Update
            if (!_isPaused)
            {
                foreach (var entity in Entities)
                    TryUpdateEntity(gameTime, entity);

                #region Entities Check
                var entities = Entities.OfType<BaseEntity>();

                foreach(var entity in entities)
                {
                    if (_toRemove.Contains(entity)) continue;

                    var bbox = entity.BoundingBox;

                    if(GameParameters.CurrentMap.CollidesWithMap(bbox))
                    {
                        _toRemove.Push(entity);
                        continue;
                    }

                    var intersect = entities.Where(e => e.BoundingBox.Intersects(bbox));

                    foreach(var intersection in intersect)
                    {
                        if(entity.TargetTag == intersection.Tag)
                        {
                            intersection.GetHit();

                            if( intersection.Health <= 0) _toRemove.Push(intersection);
                            if (entity.Tag == TypeTag.Bullet) _toRemove.Push(entity);
                        }
                    }
                }

                CleanEntities();
                #endregion Entities Check

                #region Endgame Check
                if (!_sandbox)
                {
                    if (entities.Where(e => e.Tag == TypeTag.Player).Count() == 0) Exit(Result.GameOver);
                    else if (entities.Where(e => e.Tag == TypeTag.Objective).Count() == 0) Exit(Result.LoadNext);
                }

                #endregion Endgame Check
            }
			#endregion Entity Update

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

            if (!_guiVisible)
            {
                SpriteBatch.DrawString(_font, _text, _textPosition, Color.Yellow, 0.0f, _origin, 1.0f, SpriteEffects.None, 1);
            }

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
