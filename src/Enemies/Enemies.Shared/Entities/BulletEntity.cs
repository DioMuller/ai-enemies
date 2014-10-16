using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Enemies.Entities;
using Enemies.Scripting;
using Jv.Games.Xna.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLua;
using Enemies.Parameters;

namespace Enemies.Entities
{
    public class BulletEntity : BaseEntity
    {

        #region Attributes
        Vector2 _direction;
        float _velocity;
        #endregion Attributes

        #region Constructor
        public BulletEntity(ContentManager content, TypeTag targetTag, Vector2 position, Vector2 direction, float velocity)
            : base(content, position)
        {
            this.Tag = TypeTag.Bullet;
            this.TargetTag = targetTag;

            this._direction = direction;
            this._velocity = velocity;
        }
        #endregion Constructor

        #region Game Loop
        public override void DoUpdate(float delta)
        {
            var newPos = (_direction * _velocity);
            Move(newPos.X, newPos.Y, true);

            if( GameParameters.CurrentMap.CollidesWithMap(BoundingBox) )
            {
                //TODO: Remove Entity.
            }
        }

        /// <summary>
        /// Called when the entity receives a message.
        /// </summary>
        /// <param name="message">Message received.</param>
        public override void ReceiveMessage(Message message)
        {
            SendMessage(message.Sender, "I'm a bullet!");
        }
        #endregion Game Loop
    }
}
