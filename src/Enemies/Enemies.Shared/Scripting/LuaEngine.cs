using System;
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
            yield break;
        }

        public Enemies.Entities.Entity LoadEntity(ContentManager content, string name)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
