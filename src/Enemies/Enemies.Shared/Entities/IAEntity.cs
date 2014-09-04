using Jv.Games.Xna.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enemies.Entities
{
    public class IAEntity : IEntity
    {
        #region Attributes
        #region Entity Attributes
        /// <summary>
        /// Encapsulated Entity.
        /// </summary>
        private Entity _entity;
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
        #endregion Sprite Attributes
        #endregion Attributes

        #region Properties
        /// <summary>
        /// Entity Tag.
        /// </summary>
        public string Tag { get; set; }
        
        /// <summary>
        /// Entity Position.
        /// </summary>
        public Vector2 Position
        {
            get { return Sprite.Position; }
        }
        #endregion Properties

        #region Constructor
        /// <summary>
        /// Creates an encapsulated entity with only the necessary commands exposed to the script entities.
        /// </summary>
        /// <param name="content">Content manager.</param>
        public IAEntity(ContentManager content)
        {
            _entity = new Entity();
            _content = content;
        }
        #endregion Constructor

        #region Methods

        #region Game Loop Methods
        /// <summary>
        /// Entity update logic.
        /// </summary>
        /// <param name="gameTime">Current game time.</param>
        public virtual void Update(GameTime gameTime)
        {
            (_entity as IEntity).Update(gameTime);
        }

        /// <summary>
        /// Entity drawing logic.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch used for drawing.</param>
        /// <param name="gameTime">Current game time.</param>
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
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
        public void LoadSpritesheet(string spritesheet, int cols, int rows)
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
        public void AddAnimation(string name, int line, int count, int time, bool repeat, int skipFrames)
        {
            Animation animation = _spriteSheet.GetAnimation(name, line, count, TimeSpan.FromMilliseconds(time), repeat, skipFrames);
            _entity.Sprite.Add(animation);
            _entity.Sprite.PlayAnimation(name);
        }

        public void SetCurrentAnimation(string name)
        {
            _entity.Sprite.PlayAnimation(name);
        }

        public void Move(int x, int y)
        {
            _entity.Sprite.Position.X += x;
            _entity.Sprite.Position.Y += y;
        }
        #endregion Sprite Methods

        #endregion Methods
    }
}
