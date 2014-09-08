using System;
using System.Collections.Generic;
using System.Text;
using Enemies.Screens;
using Microsoft.Scripting.Runtime;
using Microsoft.Xna.Framework;

namespace Enemies.Maps
{
    enum Tile
    {
        Ground = 0,
        Wall = 1
    }

    class Map
    {
        private Tile[,] _tiles;
        private Point _screenSize;
        private GameScreen _game;

        public Map(GameScreen game, Point screenSize)
        {
            _game = game;
            _screenSize = screenSize;
        }

        public void AddEntity(string entity, Vector2 position)
        {
            _game.AddEntity(entity, position);
        }
    }
}
