using Enemies.Entities;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Enemies.Scripting
{
    class PythonEngine : IScriptEntityFactory
    {
        readonly static ScriptEngine Engine;

        static PythonEngine()
        {
            ScriptRuntime runtime = Python.CreateRuntime(new Dictionary<string, object> {
                #if DEBUG
                { "Debug", true },
                #endif
            });
            var interopClasses = new []
            {
                typeof(Entity),
                typeof(Microsoft.Xna.Framework.Graphics.Texture2D),
                typeof(Jv.Games.Xna.Sprites.Sprite),
            };

            foreach(var interopType in interopClasses)
                runtime.LoadAssembly(Assembly.GetAssembly(interopType));

            Engine = runtime.GetEngine("py");
        }

        #region IScriptEntityFactory implementation

        public Entity LoadEntity(ContentManager content, string name)
        {
            var scriptFile = Path.Combine(content.RootDirectory, "entities", name + ".py");

            var script = Engine.CreateScriptSourceFromFile(scriptFile);
            var scope = Engine.CreateScope();
            script.Execute(scope);

            dynamic entityClass = scope.GetVariable("ScriptEntity");
            return entityClass(content);
        }

        #endregion
    }
}
