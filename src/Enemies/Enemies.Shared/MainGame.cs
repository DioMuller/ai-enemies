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
        protected GraphicsDeviceManager Graphics;
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

                    var gameResult = await baseScreen.Run(new GameScreen(this));
                    Console.WriteLine("GameScreen result: " + gameResult);
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine(ex.Message);
                    foreach (var inner in ex.InnerExceptions)
                        Console.WriteLine("Error: {0}", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex);
                }
            }

            Exit();
        }
    }
}
