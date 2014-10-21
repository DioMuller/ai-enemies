using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Enemies.Entities;
using Jv.Games.Xna.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLua;

namespace Enemies.Scripting
{
    class MapEngine : IScriptEntityFactory
    {
        public MapEngine(ContentManager content)
        {
            
        }

        #region IScriptEntityFactory implementation

        public System.Collections.Generic.IEnumerable<ScriptEntityDescription> AvailableEntities(ContentManager content)
        {
            var scriptsDir = Path.Combine(content.RootDirectory, "Maps");
            Lua context = new Lua();


            foreach (var file in Directory.GetFiles(scriptsDir, "*.xml"))
            {
                yield return new ScriptEntityDescription
                {
                    Factory = this,
                    DisplayName = Path.GetFileNameWithoutExtension(file),
                    File = Path.Combine(scriptsDir, Path.GetFileName(file))
                };
            }
        }

        public BaseEntity LoadEntity(ContentManager content, ScriptEntityDescription entity, Vector2 position)
        {
            return null;
        }

        #endregion

        public string Icon
        {
            get { return "GUI/button_over"; }
        }
    }
}
