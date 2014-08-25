using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework.Content;

namespace Enemies.Entities
{
    class PythonEntityFactory : IScriptEntityFactory
    {
        readonly static ScriptEngine Engine;

        static PythonEntityFactory()
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
            /*ScriptRuntime runtime = ScriptRuntime.Create();
            runtime.LoadAssembly( Assembly.GetAssembly( typeof( n1.MyType1)));*/

            var scriptFile = content.RootDirectory + "/entities/" + name + ".py";

            var script = Engine.CreateScriptSourceFromFile(scriptFile);
            var scope = Engine.CreateScope();
            script.Execute(scope);

            dynamic entityClass = scope.GetVariable("ScriptEntity");
            return entityClass(content);
        }

        #endregion
    }
}
