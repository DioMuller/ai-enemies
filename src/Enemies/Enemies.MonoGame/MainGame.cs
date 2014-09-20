using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Diagnostics;

namespace Enemies.MonoGame
{
    class MainGame : Enemies.MainGame
    {
        public MainGame()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600,
                IsFullScreen = false,
            };

            //IsMouseVisible = true;

            Content.RootDirectory = "Content";

            // Linux MonoGame Bug ( https://github.com/mono/MonoGame/issues/628 )
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += (sender, e) =>
            {
                Window.AllowUserResizing = false;
            };
        }
    }
}

