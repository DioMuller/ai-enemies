using System;
using System.Collections.Generic;
using System.Text;
using Jv.Games.Xna.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLua;

namespace Enemies.Entities
{
    class LuaEntity : Entity
    {
        private Lua _context;
        private ContentManager _content;
        private Texture2D _texture;
        private SpriteSheet _spriteSheet;

        public LuaFunction InitializeFunc;
        public LuaFunction UpdateFunc;

        public LuaEntity(ContentManager content, string script)
        {
            _context = new Lua();
            _context.LoadFile(script);

            _content = content;

            // Adds References
            _context.RegisterFunction("LoadSpritesheet", this, this.GetType().GetMethod("LoadSpritesheet"));
            _context.RegisterFunction("AddAnimation", this, this.GetType().GetMethod("AddAnimation"));

            // Obtains functions
            InitializeFunc = _context.GetFunction("Initialize");
            UpdateFunc = _context.GetFunction("Update");

            InitializeFunc.Call();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateFunc.Call(gameTime.ElapsedGameTime.Milliseconds);
        }

        #region Lua Calls

        private void LoadSpritesheet(string spritesheet, int cols, int rows)
        {
            _texture = _content.Load<Texture2D>(spritesheet);
            _spriteSheet = new SpriteSheet(_texture, cols, rows);
        }

        private void AddAnimation(string name, int line, int count, int time, bool repeat, int skipFrames)
        {
            Animation animation = _spriteSheet.GetAnimation(name, line, count, TimeSpan.FromMilliseconds(time), repeat, skipFrames);
            Sprite.Add(animation);
        }
        #endregion Lua Calls
    }
}
