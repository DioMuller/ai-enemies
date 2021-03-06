﻿using Jv.Games.Xna.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enemies.GUI
{
    enum CursorState
    {
        Normal,
        AddEntity,
        Build
    }

    class Cursor
    {
        #region Attributes
        private Dictionary<CursorState, Texture2D> _textures;

        private Rectangle _position;
        #endregion Attributes

        #region Properties
        public CursorState CurrentState { get; set; }

        public Point Position
        {
            get
            {
                return new Point(_position.X, _position.Y);
            }
        }

        public Rectangle CollisionRect
        {
            get { return _position; }
        }
        #endregion Properties

        #region Constructor
        public Cursor(ContentManager content)
        {
            _position = new Rectangle(0, 0, 32, 32);

            _textures = new Dictionary<CursorState, Texture2D>();
            _textures[CursorState.Normal] = content.Load<Texture2D>("GUI/cursor_pointer");
            _textures[CursorState.Build] = content.Load<Texture2D>("GUI/cursor_build");
            _textures[CursorState.AddEntity] = content.Load<Texture2D>("GUI/cursor_entity");
        }
        #endregion Constructor

        #region Game Cycle
        /// <summary>
        /// Entity update logic.
        /// </summary>
        /// <param name="gameTime">Current game time.</param>
        public void Update(GameTime gameTime)
        {
            Point position = Mouse.GetState().Position;
            _position.X = position.X;
            _position.Y = position.Y;

            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.B) || state.IsKeyDown(Keys.Enter))
            {
                CurrentState = CursorState.Build;
            }

            if (state.IsKeyDown(Keys.E) || state.IsKeyDown(Keys.A))
            {
                CurrentState = CursorState.AddEntity;
            }

            if (state.IsKeyDown(Keys.N) || state.IsKeyDown(Keys.Escape))
            {
                CurrentState = CursorState.Normal;
            }
        }

        /// <summary>
        /// Entity drawing logic.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch used for drawing.</param>
        /// <param name="gameTime">Current game time.</param>
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_textures[CurrentState], _position, null, Color.White);
            spriteBatch.End();
        }
        #endregion Game Cycle
    }
}
