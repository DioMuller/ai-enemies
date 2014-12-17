using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Enemies.Entities;
using IronPython.Modules;
using Jv.Games.Xna.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLua;

namespace Enemies.Scripting
{
    class LuaEngine : IScriptEntityFactory
    {
        public LuaEngine(ContentManager content)
        {
            
        }

        #region IScriptEntityFactory implementation

        public System.Collections.Generic.IEnumerable<ScriptEntityDescription> AvailableEntities(ContentManager content)
        {
	        var scriptsDir = new List<String>();
            Lua context = new Lua();

			scriptsDir.Add("PlayerScripts");
			scriptsDir.Add(Path.Combine(content.RootDirectory, "Scripts"));
			scriptsDir.Add(Path.Combine(content.RootDirectory, "Scripts/Entities"));

			foreach( string dir in  scriptsDir)
			{
				string[] files;

		        try
		        {
			        files = Directory.GetFiles(dir, "*.lua");
		        }
		        catch (DirectoryNotFoundException dnfex)
		        {
			        Console.WriteLine("Directory '" + dir + "' does not exist.");
					continue;
		        }

		        foreach (var file in files)
				{
					yield return new ScriptEntityDescription
					{
						Factory = this,
						DisplayName = Path.GetFileNameWithoutExtension(file),
						File = Path.Combine(dir, Path.GetFileName(file)),
                        IsPlayerCreated = (dir == "PlayerScripts")
					};
				}		
	        }
        }

        public BaseEntity LoadEntity(ContentManager content, ScriptEntityDescription entity, Vector2 position)
        {
            if (entity.Factory != this)
                throw new ArgumentException("Specified entity does not belong to this factory");

            return new LuaEntity(content, entity.File, position);
        }

        #endregion

        public string Icon
        {
            get { return "GUI/Lua"; }
        }
    }
}
