using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Enemies.Entities;
using Jv.Games.Xna.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLua;

namespace Enemies.Scripting
{
    public class LuaEngine : IScriptEntityFactory
    {
        #region Static
        public static ReadOnlyDictionary<string, LuaTable> Libs { get; private set; }

        public static void LoadLibs(ContentManager content)
        {
            if (Libs == null)
            {
                Dictionary<string, LuaTable> libs = new Dictionary<string, LuaTable>();
                Lua context = new Lua();
                
                string libpath = Path.Combine(content.RootDirectory, "Scripts/Libs/");

                foreach (var file in Directory.GetFiles(libpath, "*.lua"))
                {
                    var lib = context.DoFile(file);

                    if (lib != null)
                    {
                        libs.Add(Path.GetFileName(file).Replace(".lua", String.Empty), lib[0] as LuaTable);
                    }
                }

                Libs = new ReadOnlyDictionary<string, LuaTable>(libs);
            }
        }
        #endregion Static

        public LuaEngine(ContentManager content)
        {
            
        }

        #region IScriptEntityFactory implementation

        public System.Collections.Generic.IEnumerable<string> AvailableEntities(ContentManager content)
        {
            var scriptsDir = Path.Combine(content.RootDirectory, "Scripts/Entities");
            Lua context = new Lua();

            LoadLibs(content);

            foreach (string key in LuaEngine.Libs.Keys)
            {
                context[key] = Libs[key];
            }

            foreach (var file in Directory.GetFiles(scriptsDir, "*.lua"))
            {
                var script = context.DoFile(file);
                
                if (script != null)
                    yield return Path.GetFileName(file);
            }
        }

        public BaseEntity LoadEntity(ContentManager content, string name)
        {
            var scriptFile = Path.Combine(content.RootDirectory, "Scripts/Entities", name);

            return new LuaEntity(content, scriptFile);
        }

        #endregion
    }
}
