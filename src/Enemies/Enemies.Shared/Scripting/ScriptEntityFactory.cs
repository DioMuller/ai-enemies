using Enemies.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Enemies.Scripting
{
    class ScriptEntityFactory : IScriptEntityFactory
    {
        IDictionary<string, IScriptEntityFactory> _runtimes;

        public ScriptEntityFactory(ContentManager content)
        {
            _runtimes = new Dictionary<string, IScriptEntityFactory>
            {
                #if !__ANDROID__
                { "py", new PythonEngine(content) },
                #endif
                { "lua", new LuaEngine(content) }
            };
        }

        public BaseEntity LoadEntity(ContentManager content, ScriptEntityDescription entity, Vector2 position)
        {
            return entity.Factory.LoadEntity(content, entity, position);
        }

        public IEnumerable<ScriptEntityDescription> AvailableEntities(ContentManager content)
        {
            return _runtimes.SelectMany(r => r.Value.AvailableEntities(content));
        }

        public string Icon
        {
            get { throw new NotImplementedException(); }
        }
    }
}
