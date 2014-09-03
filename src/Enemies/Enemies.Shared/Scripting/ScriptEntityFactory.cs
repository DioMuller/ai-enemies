using Enemies.Entities;
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

        public IAEntity LoadEntity(ContentManager content, string name)
        {
            var type = Path.GetExtension(name).ToLower().TrimStart('.');

            IScriptEntityFactory factory;

            if (!_runtimes.TryGetValue(type, out factory))
                throw new InvalidOperationException("No factory registered for " + type);

            return factory.LoadEntity(content, name);
        }

        public IEnumerable<string> AvailableEntities(ContentManager content)
        {
            return _runtimes.SelectMany(r => r.Value.AvailableEntities(content));
        }
    }
}
