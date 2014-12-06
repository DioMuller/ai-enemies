using System.Threading.Tasks;
using Enemies.Media;
using Enemies.Parameters;
using Enemies.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Enemies
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class MainGame : Game
    {
        const string Tag = "MainGame";

        public GraphicsDeviceManager Graphics;
        protected SpriteBatch SpriteBatch;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            var baseScreen = new Screen<bool>(this);
            Components.Add(baseScreen);
            baseScreen.Post(GameMain);

            Jv.Games.Xna.XForms.Forms.Init(this);
            Jv.Games.Xna.XForms.Renderers.LabelRenderer.DefaultFont = Content.Load<SpriteFont>("Fonts/DefaultFont");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
			AudioPlayer.Initialize(Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public async void GameMain(Screen<bool> baseScreen)
        {
            while (true)
            {
                try
                {
                    var titleSelection = await baseScreen.Run(new TitleScreen(this));
                    Console.WriteLine("TitleScreen result: " + titleSelection);

                    if (titleSelection == TitleScreen.Result.Exit)
                        break;

                    if (titleSelection == TitleScreen.Result.StartGame)
                    {
						await LoadGame(baseScreen, "level_00", false);
                    }

                    if (titleSelection == TitleScreen.Result.StartSandbox)
                    {
	                    await LoadGame(baseScreen, "new", true);
                    }
                    if( titleSelection == TitleScreen.Result.LoadGame )
                    {
                        await LoadGame(baseScreen, "new", true, true);
                    }
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine(ex.Message);
                    ex.Handle(inner =>
                    {
                        Log.Error(Tag, inner);
                        return true;
                    });
                }
                catch (Exception ex)
                {
                    Log.Error(Tag, ex);
                }
            }

            Exit();
        }

	    public async Task LoadGame(Screen<bool> baseScreen, string level, bool sandbox, bool loadmap = false)
	    {
			var gameResult = await baseScreen.Run(new GameScreen(this, sandbox, level, loadmap));

			Console.WriteLine("GameScreen result: " + gameResult);
			if (gameResult == GameScreen.Result.LoadNext)
			{
				await LoadGame(baseScreen, GameParameters.NextMap, sandbox);
			}
			else if (gameResult == GameScreen.Result.GameOver)
			{
                await baseScreen.Run(new GameOverScreen(this, "Game Over", Color.Red) );
			}
            else if (gameResult == GameScreen.Result.Win)
            {
                await baseScreen.Run(new GameOverScreen(this, "You Win!", Color.Yellow));
            }
	    }
    }
}
