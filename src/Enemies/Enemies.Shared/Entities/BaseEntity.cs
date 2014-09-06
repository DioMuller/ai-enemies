using Jv.Games.Xna.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using Enemies.Parameters;


namespace Enemies.Entities
{
    public class BaseEntity : IEntity
    {
        #region Attributes
        #region Entity Attributes
        /// <summary>
        /// Encapsulated Entity.
        /// </summary>
        private EntityCore _entity;

        /// <summary>
        /// Entity RNG.
        /// </summary>
        private Random _random;

        #endregion Entity Attributes

        #region Sprite Attributes
        /// <summary>
        /// Content Manager.
        /// </summary>
        private ContentManager _content;

        /// <summary>
        /// Entity Texture.
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// Entity Spritesheet.
        /// </summary>
        private SpriteSheet _spriteSheet;

        /// <summary>
        /// Idle animation name.
        /// </summary>
        private string _idleAnimation = "";

        /// <summary>
        /// Moving animation name.
        /// </summary>
        private string _movingAnimation = "";

        #endregion Sprite Attributes
        #endregion Attributes

        #region Properties
        /// <summary>
        /// Entity Tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Target Tag.
        /// </summary>
        public string TargetTag { get; set; }

        /// <summary>
        /// Enemy Tag.
        /// </summary>
        public string EnemyTag { get; set; }
        
        /// <summary>
        /// Entity Position.
        /// </summary>
        public Vector2 Position
        {
            get { return _entity.Sprite.Position; }
        }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Creates an encapsulated entity with only the necessary commands exposed to the script entities.
        /// </summary>
        /// <param name="content">Content manager.</param>
        public BaseEntity(ContentManager content)
        {
            _entity = new EntityCore();
            _content = content;
            _random = new Random();

            Initialize();
        }
        #endregion Constructor

        #region Game Loop Methods
        /// <summary>
        /// Script initialization logic.
        /// </summary>
        public virtual void Initialize()
        {

        }

        public virtual void DoUpdate(float delta)
        {

        }

        
        /// <summary>
        /// Entity update logic.
        /// </summary>
        /// <param name="gameTime">Current game time.</param>
        public void Update(GameTime gameTime)
        {
            (_entity as IEntity).Update(gameTime);
            DoUpdate(gameTime.ElapsedGameTime.Milliseconds);
        }

        /// <summary>
        /// Entity drawing logic.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch used for drawing.</param>
        /// <param name="gameTime">Current game time.</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            (_entity as IEntity).Draw(spriteBatch, gameTime);
        }
        #endregion Game Loop Methods

        #region Sprite Methods
        /// <summary>
        /// Loads entity spritesheet.
        /// </summary>
        /// <param name="spritesheet">Spritesheet file.</param>
        /// <param name="cols">Number of columns on the spritesheet.</param>
        /// <param name="rows">Number of rows on the spritesheet.</param>
        private void LoadSpritesheet(string spritesheet, int cols, int rows)
        {
            _texture = _content.Load<Texture2D>(spritesheet);
            _spriteSheet = new SpriteSheet(_texture, cols, rows);
        }

        /// <summary>
        /// Adds animation to the spritesheet.
        /// </summary>
        /// <param name="name">Animation name.</param>
        /// <param name="line">Animation line.</param>
        /// <param name="count">Frame count.</param>
        /// <param name="time">Time interval between frame changes.</param>
        /// <param name="repeat">Specifies if the animation loop does loop.</param>
        /// <param name="skipFrames">Number of frames ignored on the line.</param>
        private void AddAnimation(string name, int line, int count, int time, bool repeat, int skipFrames)
        {
            Animation animation = _spriteSheet.GetAnimation(name, line, count, TimeSpan.FromMilliseconds(time), repeat, skipFrames);
            _entity.Sprite.Add(animation);
            _entity.Sprite.PlayAnimation(name);
        }

        #endregion Sprite Methods

        #region Helper Methods
        /// <summary>
        /// Gets Entities with the specified tag. 
        /// If tag is null, will return all the entities.
        /// </summary>
        /// <param name="tag">Entity tag.</param>
        /// <returns></returns>
        private EntityInfo[] GetNeighbours(string tag = null)
        {
            var entities = GameParameters.Entities;

            if (tag != null) return entities.Where((e) => e.Tag == tag).ToArray();
            else return entities.ToArray();
        }
        #endregion Helper Methods

        #region Exposed Methods
        /// <summary>
        /// Loads spritesheet from an xml file definition.
        /// </summary>
        /// <param name="file">Spritesheet definition file name (without extension).</param>
        public void SetSpritesheet(string file)
        {
            string path = Path.Combine(_content.RootDirectory, file.Replace('/','\\') + ".xml");
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Element("sprite");

            #region Load Spritesheets
            var sheets = root.Element("spritesheets");
            int columns = Convert.ToInt32(sheets.Attribute("columns").Value);
            int rows = Convert.ToInt32(sheets.Attribute("rows").Value);
            List<string> files = new List<string>();

            foreach(var sheet in sheets.Elements("spritesheet"))
            {
                files.Add(sheet.Attribute("file").Value);
            }

            if( files.Count > 0)
            {
                // Chooses an random spritesheet from the defined ones.
                LoadSpritesheet(files[_random.Next(0, files.Count)], columns, rows);
            }   
            #endregion Load Spritesheets

            #region Load Animations
            var animations = root.Element("animations");
            _idleAnimation = animations.Attribute("idleAnimation").Value;
            _movingAnimation = animations.Attribute("movingAnimation").Value;

            foreach (var animation in animations.Elements("animation"))
            {
                string name = animation.Attribute("name").Value;
                int line = Convert.ToInt32(animation.Attribute("line").Value);
                int count = Convert.ToInt32(animation.Attribute("count").Value); ;
                int time = Convert.ToInt32(animation.Attribute("time").Value); ;
                bool repeat = Convert.ToBoolean(animation.Attribute("repeat").Value); ;
                int skipFrames = Convert.ToInt32(animation.Attribute("skipFrames").Value); ;

                AddAnimation(name, line, count, time, repeat, skipFrames);
            }
            #endregion Load Animations
        }

        /// <summary>
        /// Changes the current character animation.
        /// </summary>
        /// <param name="name">Animation name.</param>
        public void SetCurrentAnimation(string name)
        {
            _entity.Sprite.PlayAnimation(name);
        }

        /// <summary>
        /// Moves entity.
        /// </summary>
        /// <param name="x">Movement X.</param>
        /// <param name="y">Movement Y.</param>
        public void Move(float x, float y)
        {
            _entity.Sprite.Position.X += x;
            _entity.Sprite.Position.Y += y;
        }

        /// <summary>
        /// Obtains the nearest target.
        /// </summary>
        /// <returns>Nearest target info.</returns>
        public EntityInfo GetNearestTarget()
        {

            return GetNeighbours(TargetTag).OrderBy(e => GetDistanceFrom(e)).FirstOrDefault();
        }

        /// <summary>
        /// Obtains the nearest target.
        /// </summary>
        /// <returns>Nearest target info.</returns>
        public EntityInfo[] GetNearbyEnemies()
        {

            return GetNeighbours(EnemyTag).OrderBy(e => GetDistanceFrom(e)).ToArray();
        }

        /// <summary>
        /// Obtains entity info.
        /// </summary>
        /// <returns>The entity info.</returns>
        public EntityInfo GetInfo()
        {
            return new EntityInfo(Tag, _entity.Sprite.Position);
        }

        /// <summary>
        /// Obtains the squared distance from another entity.
        /// </summary>
        /// <returns>Distance from the other entity.</returns>
        public float GetDistanceFrom(EntityInfo entity)
        {
            return (Position.X - entity.Position.X)*(Position.X - entity.Position.X) +
                   (Position.Y - entity.Position.Y)*(Position.Y - entity.Position.Y);
        }
        #endregion Methods
    }
}
