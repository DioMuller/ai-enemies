using System;
using System.Collections.Generic;
using System.IO;
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

        public LuaEntity(ContentManager content, string script) : base()
        {
            _context = new Lua();
            _context.LoadCLRPackage();

            if(File.Exists(script))
                _context["script"] = _context.DoFile(script)[0] as LuaTable;

            _context["entity"] = this;

            _content = content;

            // Obtains functions
            InitializeFunc = (_context["script"] as LuaTable)["Initialize"] as LuaFunction;
            UpdateFunc = (_context["script"] as LuaTable)["Update"] as LuaFunction;

            InitializeFunc.Call();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateFunc.Call(gameTime.ElapsedGameTime.Milliseconds);
        }

        #region Lua Calls

        public void LoadSpritesheet(string spritesheet, int cols, int rows)
        {
            _texture = _content.Load<Texture2D>(spritesheet);
            _spriteSheet = new SpriteSheet(_texture, cols, rows);
        }

        public void AddAnimation(string name, int line, int count, int time, bool repeat, int skipFrames)
        {
            Animation animation = _spriteSheet.GetAnimation(name, line, count, TimeSpan.FromMilliseconds(time), repeat, skipFrames);
            Sprite.Add(animation);
            Sprite.PlayAnimation(name);
        }

        public void SetCurrentAnimation(string name)
        {
            Sprite.PlayAnimation(name);
        }
        #endregion Lua Calls
    }
}
