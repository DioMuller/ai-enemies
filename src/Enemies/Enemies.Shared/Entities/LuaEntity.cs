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
    class LuaEntity : BaseEntity
    {
        #region Attributes
        private Lua _context;

        private LuaFunction InitializeFunc;
        private LuaFunction UpdateFunc;
        #endregion Attributes

        #region Constructor
        public LuaEntity(ContentManager content, string script) : base(content)
        {
            _context = new Lua();
            _context.LoadCLRPackage();

            if(File.Exists(script))
                _context["script"] = _context.DoFile(script)[0] as LuaTable;

            _context["entity"] = this;

            // Obtains functions
            InitializeFunc = (_context["script"] as LuaTable)["Initialize"] as LuaFunction;
            UpdateFunc = (_context["script"] as LuaTable)["Update"] as LuaFunction;

            InitializeFunc.Call();
        }
        #endregion Constructor

        #region Game Loop
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateFunc.Call(gameTime.ElapsedGameTime.Milliseconds);
        }
        #endregion Game Loop
    }
}
