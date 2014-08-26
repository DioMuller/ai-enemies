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

        public ScriptEntityFactory()
        {
            _runtimes = new Dictionary<string, IScriptEntityFactory> {
                { "py", new PythonEngine() }
            };
        }

        public Entity LoadEntity(ContentManager content, string name)
        {
            var dir = Path.Combine(content.RootDirectory, "entities");
            var file = Directory.GetFiles(dir, name + "*").FirstOrDefault();
            var type = Path.GetExtension(file).ToLower().TrimStart('.');

            IScriptEntityFactory factory;

            if (!_runtimes.TryGetValue(type, out factory))
                throw new InvalidOperationException("No factory registered for " + type);

            return factory.LoadEntity(content, name);
        }
    }
}
