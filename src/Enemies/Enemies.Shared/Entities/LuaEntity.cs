using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Enemies.Scripting;
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

            Tag = "Lua";

            foreach (string key in LuaEngine.Libs.Keys)
            {
                _context[key] = LuaEngine.Libs[key];
            }

            if(File.Exists(script))
                _context["script"] = _context.DoFile(script)[0] as LuaTable;

            _context["entity"] = this;

            // Obtains functions
            InitializeFunc = (_context["script"] as LuaTable)["Initialize"] as LuaFunction;
            UpdateFunc = (_context["script"] as LuaTable)["DoUpdate"] as LuaFunction;

            InitializeFunc.Call();
        }
        #endregion Constructor

        #region Game Loop
        public override void DoUpdate(float delta)
        {
            base.DoUpdate(delta);

            UpdateFunc.Call(delta);
        }
        #endregion Game Loop
    }
}
