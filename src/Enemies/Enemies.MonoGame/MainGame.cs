using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Enemies.MonoGame
{
    public class MainGame : Enemies.MainGame
    {
        public MainGame()
        {
            Graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600,
                IsFullScreen = false,
            };

            Content.RootDirectory = "Content";

            // Linux MonoGame Bug ( https://github.com/mono/MonoGame/issues/628 )
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += (sender, e) => {
                Window.AllowUserResizing = false;
            };
        }
    }
}

