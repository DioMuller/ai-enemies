using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Enemies.Entities;
using Enemies.Screens;
using Microsoft.Scripting.Runtime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

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

        public Map(GameScreen game, Point screenSize, ContentManager content, string file)
        {
            _game = game;
            _screenSize = screenSize;

            LoadFromFile(content, file);
        }

        public void AddEntity(string entity, Vector2 position, TypeTag tag)
        {
            _game.AddEntity(entity, position, tag);
        }

        protected void LoadFromFile(ContentManager content, string file)
        {
            string path = Path.Combine(content.RootDirectory, file.Replace('/', '\\') + ".xml");
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Element("level");

            #region Load Map
            var map = root.Element("map");
            int height = Convert.ToInt32(map.Attribute("height").Value);
            int width = Convert.ToInt32(map.Attribute("width").Value);
            _tiles = new Tile[height,width];
            var mapValues = map.Value.Split('\n').Select(s => s.Trim());
            #endregion Load Spritesheets

            #region Load Entities
            var entities = root.Element("entities");

            foreach (var entity in entities.Elements("entity"))
            {
                string script = entity.Attribute("script").Value;
                int x = Convert.ToInt32(entity.Attribute("x").Value);
                int y = Convert.ToInt32(entity.Attribute("y").Value);
                TypeTag tag = TypeTag.None;

                switch (entity.Attribute("tag").Value)
                {
                    case "player":
                        tag = TypeTag.Player;
                        break;
                    case "enemy":
                        tag = TypeTag.Enemy;
                        break;
                    case "objective":
                        tag = TypeTag.Objective;
                        break;
                    default:
                        tag = TypeTag.None;
                        break;
                }

                if (tag != TypeTag.None)
                {
                    AddEntity(script, new Vector2(x,y), tag );
                }

            }
            #endregion Load Entities
        }
    }
}
