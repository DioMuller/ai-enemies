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
using Microsoft.Xna.Framework.Graphics;

namespace Enemies.Maps
{
    enum Tile
    {
        Ground = '0',
        Wall = '1',
        Empty = ' '
    }

    class Map
    {
        private Tile[,] _tiles;
        private Point _screenSize;
        private GameScreen _game;

        private Vector2 _tileSize;
        private List<Rectangle> _collisions;

        #region Textures

        private Texture2D _wallTexture = null;
        private Texture2D _groundTexture = null;
        private Texture2D _emptyTexture = null;

        #endregion Textures

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
            file = file.Replace('/', '\\') + ".xml";
            string path = Path.Combine(content.RootDirectory, file );
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Element("level");

            _collisions = new List<Rectangle>();

            #region Load Map
            var map = root.Element("map");
            int height = Convert.ToInt32(map.Attribute("height").Value);
            int width = Convert.ToInt32(map.Attribute("width").Value);
            _tileSize = new Vector2(_screenSize.X/width, _screenSize.Y/height);
            _tiles = new Tile[height,width];
            var mapValues = map.Value.Split('\n').Select(s => s.Trim()).Where(s => !String.IsNullOrEmpty(s) ).ToArray();

            for (int i = 0; i < height; i++)
            {
                if(mapValues.Length > i)
                { 
                    char[] mapTiles = mapValues[i].ToCharArray();

                    for (int j = 0; j < width; j++)
                    {
                        char type = mapTiles[j];
                        if (type == '0' || type == '1') _tiles[i, j] = (Tile) type;
                        else _tiles[i, j] = Tile.Empty;

                        if( _tiles[i,j] == Tile.Wall || _tiles[i,j] == Tile.Empty)
                        {
                            _collisions.Add(new Rectangle(Convert.ToInt32(j * _tileSize.X), 
                                Convert.ToInt32(i * _tileSize.Y), 
                                Convert.ToInt32(_tileSize.X), Convert.ToInt32(_tileSize.Y)));
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < width; j++)
                    {
                        _tiles[i, j] = Tile.Empty;
                        _collisions.Add(new Rectangle(Convert.ToInt32(j * _tileSize.X),
                                Convert.ToInt32(i * _tileSize.Y),
                                Convert.ToInt32(_tileSize.X), Convert.ToInt32(_tileSize.Y)));
                    }
                }
            }
            #endregion Load Map

            #region Load "Tileset"
            var tiles = root.Element("tiles");
            _groundTexture = content.Load<Texture2D>(tiles.Attribute("ground").Value);
            _wallTexture = content.Load<Texture2D>(tiles.Attribute("wall").Value);
            _emptyTexture = content.Load<Texture2D>(tiles.Attribute("empty").Value);
            #endregion Load "Tileset"

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

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < _tiles.GetLength(1); i++)
            {
                for (int j = 0; j < _tiles.GetLength(0); j++)
                {
                    Rectangle destRectangle = new Rectangle(Convert.ToInt32(i * _tileSize.X), 
                        Convert.ToInt32(j * _tileSize.Y), 
                        Convert.ToInt32(_tileSize.X), 
                        Convert.ToInt32(_tileSize.Y));
                    switch (_tiles[j,i])
                    {
                        case Tile.Ground:
                            spriteBatch.Draw(_groundTexture, destRectangle, null, Color.White);
                            break;
                        case Tile.Wall:
                            spriteBatch.Draw(_wallTexture, destRectangle, null, Color.White);
                            break;
                        default:
                            spriteBatch.Draw(_emptyTexture, destRectangle, null, Color.White);
                            break;
                    }
                    
                }
            }
        }

        public bool CollidesWithMap(Rectangle rect)
        {
            var collisions = _collisions.Where(r => r.Intersects(rect));

            return collisions.Count() > 0;
        }

        public Point GetQuadrant(Point position)
        {
            return new Point(Convert.ToInt32(position.X / _tileSize.X),
                Convert.ToInt32(position.Y / _tileSize.Y));
        }

        public void ChangeQuadrant(Point quadrant, Tile tile)
        {
            if (quadrant.X < 0 || quadrant.X > _tiles.GetLength(0)) return;
            if (quadrant.Y < 0 || quadrant.Y > _tiles.GetLength(1)) return;

            _tiles[quadrant.Y, quadrant.X] = tile;
        }

        public Rectangle GetRectanglePosition(Vector2 position)
        {
            return new Rectangle(Convert.ToInt32(position.X * _tileSize.X),
                                Convert.ToInt32(position.Y * _tileSize.Y),
                                Convert.ToInt32(_tileSize.X), Convert.ToInt32(_tileSize.Y));
        }
    }
}
