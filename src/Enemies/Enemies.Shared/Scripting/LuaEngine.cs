using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Enemies.Entities;
using Jv.Games.Xna.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLua;

namespace Enemies.Scripting
{
    public class LuaEngine : IScriptEntityFactory
    {
        public LuaEngine(ContentManager content)
        {
            
        }

        #region IScriptEntityFactory implementation

        public System.Collections.Generic.IEnumerable<string> AvailableEntities(ContentManager content)
        {
            var scriptsDir = Path.Combine(content.RootDirectory, "Scripts/Entities");
            Lua context = new Lua();


            foreach (var file in Directory.GetFiles(scriptsDir, "*.lua"))
            {
                yield return Path.GetFileName(file);
            }
        }

        public BaseEntity LoadEntity(ContentManager content, string name, Vector2 position)
        {
            var scriptFile = Path.Combine(content.RootDirectory, "Scripts/Entities", name);

            return new LuaEntity(content, scriptFile, position );
        }

        #endregion
    }
}
