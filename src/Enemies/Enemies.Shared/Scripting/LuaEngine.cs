using System;
using System.IO;
using Enemies.Entities;
using Microsoft.Xna.Framework.Content;
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
            var scriptsDir = Path.Combine(content.RootDirectory, "scripts");
            foreach (var file in Directory.GetFiles(scriptsDir, "entity_*.lua"))
            {
                yield return Path.GetFileName(file);
            }
        }

        public Enemies.Entities.Entity LoadEntity(ContentManager content, string name)
        {
            var scriptFile = Path.Combine(content.RootDirectory, "scripts", name);

            return new LuaEntity(scriptFile);
        }

        #endregion
    }
}
