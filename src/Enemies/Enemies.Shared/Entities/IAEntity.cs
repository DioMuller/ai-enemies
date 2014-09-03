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
        private Entity _entity;
        #endregion Entity Attributes

        #region Sprite Attributes
        private ContentManager _content;
        private Texture2D _texture;
        private SpriteSheet _spriteSheet;
        #endregion Sprite Attributes
        #endregion Attributes

        #region Constructor
        public IAEntity(ContentManager content)
        {
            _entity = new Entity();
            _content = content;
        }
        #endregion Constructor

        #region Methods

        #region Game Loop Methods
        public virtual void Update(GameTime gameTime)
        {
            (_entity as IEntity).Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            (_entity as IEntity).Draw(spriteBatch, gameTime);
        }
        #endregion Game Loop Methods

        #region Sprite Methods
        public void LoadSpritesheet(string spritesheet, int cols, int rows)
        {
            _texture = _content.Load<Texture2D>(spritesheet);
            _spriteSheet = new SpriteSheet(_texture, cols, rows);
        }

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
