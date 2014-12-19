using System.Linq;
using Microsoft.Xna.Framework;
#if !__ANDROID__
using Enemies.Entities;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;

namespace Enemies.Scripting
{
    class PythonEngine : IScriptEntityFactory
    {
        readonly ScriptEngine Engine;

        public PythonEngine(ContentManager content)
        {
            ScriptRuntime runtime = Python.CreateRuntime(new Dictionary<string, object>
            {
#if DEBUG
                { "Debug", true },
#endif
            });
            var interopClasses = new[]
            {
                typeof(BaseEntity),
                typeof(Microsoft.Xna.Framework.Graphics.Texture2D),
                typeof(Jv.Games.Xna.Sprites.Sprite),
            };

            foreach (var interopType in interopClasses)
                runtime.LoadAssembly(Assembly.GetAssembly(interopType));

            Engine = runtime.GetEngine("py");
            var paths = Engine.GetSearchPaths();
			paths.Add("PlayerScripts");
            paths.Add(Path.Combine(content.RootDirectory, "Scripts"));
            paths.Add(Path.Combine(content.RootDirectory, "Scripts/Entities"));
            Engine.SetSearchPaths(paths);
        }

        #region IScriptEntityFactory implementation

        public BaseEntity LoadEntity(ContentManager content, ScriptEntityDescription entity, Vector2 position)
        {
            if (entity.Factory != this)
                throw new ArgumentException("Specified entity does not belong to this factory");

            var script = Engine.CreateScriptSourceFromFile(entity.File);
            var scope = Engine.CreateScope();
            script.Execute(scope);

            dynamic entityClass = scope.GetVariable("ScriptEntity");
            return entityClass(content, position);
        }

	    public IEnumerable<ScriptEntityDescription> AvailableEntities(ContentManager content)
	    {
		    string[] scriptsDirs = {Path.Combine(content.RootDirectory, "Scripts/Entities"),"PlayerScripts"};

			foreach (var scriptsDir in scriptsDirs)
            {
                string[] files;

                try
                {
                    files = Directory.GetFiles(scriptsDir, "*.py");
                }
                catch (DirectoryNotFoundException dnfex)
                {
                    Console.WriteLine("Directory '" + scriptsDir + "' does not exist.");
                    continue;
                }

                foreach (var file in files)
                {
                    var script = Engine.CreateScriptSourceFromFile(file);
                    var scope = Engine.CreateScope();
                    script.Execute(scope);

                    if (!scope.ContainsVariable("ScriptEntity"))
                        continue;

                    yield return new ScriptEntityDescription
                    {
                        Factory = this,
                        DisplayName = Path.GetFileNameWithoutExtension(file),
                        File = Path.Combine(scriptsDir, Path.GetFileName(file)),
                        IsPlayerCreated = (scriptsDir == "PlayerScripts")
                    };
                }
            }
        }

        #endregion

        public string Icon
        {
            get { return "GUI/Python"; }
        }
    }
}
#endif
