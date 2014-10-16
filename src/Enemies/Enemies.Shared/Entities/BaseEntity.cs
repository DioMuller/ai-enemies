using System.Dynamic;
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
    public enum TypeTag
    {
        Enemy = 0,
        Player = 1,
        Objective = 2,
        Bullet = 4,
        None = 3
    }

    public class BaseEntity : IEntity
    {
        #region Static
        private static int CurrentId = 0;
	    private static SpriteFont _font = null;
        #endregion Static

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

        /// <summary>
        /// Current type tag.
        /// </summary>
        private TypeTag _tag = TypeTag.None;

        /// <summary>
        /// Entity has moved this update cycle?
        /// </summary>
        private bool _hasMoved = false;
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
        private GridSpriteSheet _spriteSheet;

        /// <summary>
        /// Idle animation name.
        /// </summary>
        private string _idleAnimation = "";

        /// <summary>
        /// Moving animation name.
        /// </summary>
        private string _movingAnimation = "";

		/// <summary>
		/// Current Entity dialog.
		/// </summary>
	    private string _currentDialog = String.Empty;

		/// <summary>
		/// Time since last dialog.
		/// </summary>
	    private float _timeSinceLastDialog = 0.0f;
        #endregion Sprite Attributes
        #endregion Attributes

        #region Properties

        /// <summary>
        /// Entity Tag.
        /// </summary>
        public TypeTag Tag
        {
            get { return _tag; }
            set
            {
                _tag = value;

                switch (_tag)
                {
                    case TypeTag.Enemy:
                        EnemyTag = TypeTag.None;
                        TargetTag = TypeTag.Player;
                        SetSpritesheet("Sprites/Zombie");
                        break;
                    case TypeTag.Player:
                        EnemyTag = TypeTag.Enemy;
                        TargetTag = TypeTag.Objective;
                        SetSpritesheet("Sprites/Human");
                        break;
                    case TypeTag.Objective:
                        EnemyTag = TypeTag.Player;
                        TargetTag = TypeTag.None;
                        SetSpritesheet("Sprites/Human");
                        break;
                    default:
                        EnemyTag = TypeTag.None;
                        TargetTag = TypeTag.None;
                        SetSpritesheet("Sprites/Human");
                        break;
                }

                _entity.Sprite.Color = GetColor(_tag);
            }
        }

        /// <summary>
        /// Target Tag.
        /// </summary>
        public TypeTag TargetTag { get; protected set; }
                
        /// <summary>
        /// Enemy Tag.
        /// </summary>
        public TypeTag EnemyTag { get; private set; }
        
        /// <summary>
        /// Entity Position.
        /// </summary>
        public Vector2 Position
        {
            get { return _entity.Sprite.Position; }
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle
                (
                    Convert.ToInt32(Position.X - _entity.Sprite.CurrentAnimation.Frames[0].Origin.X),
                    Convert.ToInt32(Position.Y - _entity.Sprite.CurrentAnimation.Frames[0].Origin.Y),
                    _entity.Sprite.CurrentAnimation.Frames[0].Width,
                    _entity.Sprite.CurrentAnimation.Frames[0].Height
                );
            }
        }

        /// <summary>
        /// Entity Id.
        /// </summary>
        public int Id { get; private set; }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Creates an encapsulated entity with only the necessary commands exposed to the script entities.
        /// </summary>
        /// <param name="content">Content manager.</param>
        public BaseEntity(ContentManager content, Vector2 position)
        {
            _entity = new EntityCore();
            _content = content;
            _random = new Random();

            Id = (CurrentId++);

            _entity.Sprite.Position = position;

	        if (_font == null)
	        {
		        _font = content.Load<SpriteFont>("Fonts/DefaultFont");
	        }

            Initialize();
        }
        #endregion Constructor

        #region Exposed Game Loop Methods
        /// <summary>
        /// Script initialization logic.
        /// </summary>
        public virtual void Initialize()
        {

        }

        /// <summary>
        /// Executed on the Update Loop.
        /// </summary>
        /// <param name="delta"></param>
        public virtual void DoUpdate(float delta)
        {

        }

        /// <summary>
        /// Called when the entity receives a message.
        /// </summary>
        /// <param name="message">Message received.</param>
        public virtual void ReceiveMessage(Message message)
        {

        }
        #endregion Exposed Game Loop Methods

        #region Game Loop Methods

        /// <summary>
        /// Entity update logic.
        /// </summary>
        /// <param name="gameTime">Current game time.</param>
        public void Update(GameTime gameTime)
        {
	        if (_timeSinceLastDialog > 0.0f)
	        {
		        _timeSinceLastDialog -= gameTime.ElapsedGameTime.Milliseconds;
	        }
	        else
	        {
		        _currentDialog = String.Empty;
	        }

            (_entity as IEntity).Update(gameTime);
            DoUpdate(gameTime.ElapsedGameTime.Milliseconds);

            _hasMoved = false;
        }

        /// <summary>
        /// Entity drawing logic.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch used for drawing.</param>
        /// <param name="gameTime">Current game time.</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Rectangle baseRect = BoundingBox; //new Rectangle(BoundingBox.X + BoundingBox.Width - 48, BoundingBox.Y + BoundingBox.Height - 24, 32, 32);


	        if (_currentDialog != String.Empty)
	        {
		        var measurements = _font.MeasureString(_currentDialog) * 0.7f;
		        Vector2 textpos = new Vector2(
			        ((Position.X + BoundingBox.Width/2.0f) - measurements.X/2),
			        ((Position.Y + BoundingBox.Height/2.0f) - (measurements.Y + 25)));

		        spriteBatch.DrawString(_font, _currentDialog, textpos, Color.White, 0.0f, new Vector2(0,0), new Vector2(0.7f, 0.7f), SpriteEffects.None, 1.0f  );
	        }

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
            _spriteSheet = new GridSpriteSheet(_texture, cols, rows);
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

            foreach (var frame in animation.Frames)
            {
                frame.Origin = new Vector2(
                    frame.Rectangle.Width / 2,
                    frame.Rectangle.Height / 2);
            }

            _entity.Sprite.Add(animation);
            _entity.Sprite.PlayAnimation(name);
        }

        /// <summary>
        /// Loads spritesheet from an xml file definition.
        /// </summary>
        /// <param name="file">Spritesheet definition file name (without extension).</param>
        private void SetSpritesheet(string file)
        {
            _entity.Sprite.Clear();

            string path = Path.Combine(_content.RootDirectory, file.Replace('/', '\\') + ".xml");
            XDocument doc = XDocument.Load(path);
            XElement root = doc.Element("sprite");

            #region Load Spritesheets
            var sheets = root.Element("spritesheets");
            int columns = Convert.ToInt32(sheets.Attribute("columns").Value);
            int rows = Convert.ToInt32(sheets.Attribute("rows").Value);
            List<string> files = new List<string>();

            foreach (var sheet in sheets.Elements("spritesheet"))
            {
                files.Add(sheet.Attribute("file").Value);
            }

            if (files.Count > 0)
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
        private void SetCurrentAnimation(string name)
        {
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
        private EntityInfo[] GetNeighbours(TypeTag tag = TypeTag.None)
        {
            var entities = GameParameters.Entities;

            if (entities == null) return null;

            if (tag != TypeTag.None) return entities.Where((e) => e.Tag == tag).ToArray();
            else return entities.ToArray();
        }

        /// <summary>
        /// Obtains color related to tag.
        /// </summary>
        /// <param name="tag">Entity tag.</param>
        /// <returns></returns>
        private Color GetColor(TypeTag tag)
        {
            switch(tag)
            {
                case TypeTag.Enemy:
                    return Color.Red;
                case TypeTag.Player:
                    return Color.Green;
                case TypeTag.Objective:
                    return Color.Yellow;
                default:
                    return Color.White;
            }
        }
        #endregion Helper Methods

        #region Exposed Methods
        /// <summary>
        /// Moves entity.
        /// </summary>
        /// <param name="x">Movement X.</param>
        /// <param name="y">Movement Y.</param>
        public void Move(float x, float y, bool rotate = false)
        {
            if (_hasMoved) return;

            Vector2 normalized = new Vector2(x, y);
            Vector2 oldPos = Position;

            if (this.Tag != TypeTag.Bullet)
            {
                normalized.Normalize();
            }

            if (float.IsNaN(normalized.X) || float.IsNaN(normalized.Y))
            {
                //Log.Debug("Entity", "Entity " + this.GetType().Name + " tried to move to an NaN position.");
                return;
            }            
                
            _entity.Sprite.Position.X += normalized.X;

            if( rotate )
            {
                var lookAtPos = normalized * 100;

                LookAt(lookAtPos.X, lookAtPos.Y);
            }

            if (GameParameters.CurrentMap.CollidesWithMap(BoundingBox) && Tag != TypeTag.Bullet )
            {
                _entity.Sprite.Position.X = oldPos.X;
            }

            _entity.Sprite.Position.Y += normalized.Y;

            if (GameParameters.CurrentMap.CollidesWithMap(BoundingBox) && Tag != TypeTag.Bullet)
            {
                _entity.Sprite.Position.Y = oldPos.Y;
            }

            _hasMoved = _entity.Sprite.Position != oldPos;
        }

        /// <summary>
        /// Sets the entity rotation.
        /// </summary>
        /// <param name="angle">Rotation angle.</param>
        public void SetRotation(float angle)
        {
            _entity.Sprite.Rotation = angle;
        }

        /// <summary>
        /// Makes the character look at the position.
        /// </summary>
        /// <param name="x">Position X.</param>
        /// <param name="y">Position Y.</param>
        public void LookAt(float x, float y)
        {
            SetRotation((float) Math.Atan2( x, -y));
        }

        /// <summary>
        /// Moves entity to the specific direction.
        /// </summary>
        /// <param name="x">Objective X position.</param>
        /// <param name="y">Objective Y position.</param>
        /// <rreturns>Has the entity arrived?</rreturns>
        public bool MoveTo(float x, float y)
        {
            Move(x - _entity.Sprite.Position.X,  y - _entity.Sprite.Position.Y, true);

            return _entity.Sprite.Position.X == x && _entity.Sprite.Position.Y == y;
        }

        /// <summary>
        /// Obtains the nearest target.
        /// </summary>
        /// <returns>Nearest target info.</returns>
        public EntityInfo GetNearestTarget()
        {
            var neigh = GetNeighbours(TargetTag);
            if (neigh == null) return null;

            return neigh.OrderBy(e => GetDistanceFrom(e)).FirstOrDefault();
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
            return new EntityInfo(Id, Tag, _entity.Sprite.Position);
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

        /// <summary>
        /// Sends a message to one entity (by id)
        /// </summary>
        /// <param name="receiver">Receiver entity id.</param>
        /// <param name="message">Message to be sent.</param>
        /// <param name="attachment">Attached object (optional).</param>
        public void SendMessage(int receiver, string message, object attachment = null)
        {
            MessageManager.SendMessage(Id, receiver, message, attachment);
        }

        /// <summary>
        /// Broadcasts message to all entities.
        /// </summary>
        /// <param name="message">Message to broadcast</param>
        /// <param name="attachment">Attached object (optional).</param>
        public void BroadcastMessage(string message, object attachment = null)
        {
            MessageManager.SendMessage(Id, -1, message, attachment);
        }

		/// <summary>
		/// Makes the entity talk.
		/// </summary>
		/// <param name="message">Message to talk.</param>
	    public void Talk(string message)
	    {
		    _timeSinceLastDialog = 1000.0f;
		    _currentDialog = message;
	    }
        #endregion Methods
    }
}
