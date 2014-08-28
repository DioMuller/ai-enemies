using System;
using System.IO;
using Microsoft.Xna.Framework.Content;
using NLua;

namespace Enemies.Scripting
{
    public class LuaEngine : IScriptEntityFactory
    {
        Lua context;

        public LuaEngine(ContentManager content)
        {
            context = new Lua();
        }

        #region IScriptEntityFactory implementation

        public System.Collections.Generic.IEnumerable<string> AvailableEntities(ContentManager content)
        {
            var scriptsDir = Path.Combine(content.RootDirectory, "scripts");
            foreach (var file in Directory.GetFiles(scriptsDir, "entity_*.lua"))
            {
                // TODO: Load Scripts?
                yield return Path.GetFileName(file);
            }
        }

        public Enemies.Entities.Entity LoadEntity(ContentManager content, string name)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
