﻿using System;
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

namespace Enemies.Entities
{
    public class LuaEntity : BaseEntity
    {

        #region Libs

        private Dictionary<string, LuaTable> _libs = null;

        private void LoadLibs(ContentManager content)
        {
            // If libs are not loaded.
            if( _libs == null )
            {
                _libs = new Dictionary<string, LuaTable>();

                string libpath = Path.Combine(content.RootDirectory, "Scripts/Libs/");

                foreach (var file in Directory.GetFiles(libpath, "*.lua"))
                {
                    var lib = _context.DoFile(file);

                    if (lib != null)
                    {
                        _libs.Add(Path.GetFileName(file).Replace(".lua", String.Empty), lib[0] as LuaTable);
                    }
                }
            }

            // Add libs to context.
            foreach (string key in _libs.Keys)
            {
                _context[key] = _libs[key];
            }
        }
        #endregion Libs

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

            LoadLibs(content);

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

        #region Lua Specific Exposed Methods

        public void InitializeValue(string key, object value)
        {
            (_context["script"]as LuaTable)[key] = value;
        }
        #endregion Lua Specific Exposed Methods
    }
}
