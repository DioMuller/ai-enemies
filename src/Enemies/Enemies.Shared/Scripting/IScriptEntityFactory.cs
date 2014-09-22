using Enemies.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Enemies.Scripting
{
    public interface IScriptEntityFactory
    {
        string Icon { get; }

        BaseEntity LoadEntity(ContentManager content, ScriptEntityDescription entity, Vector2 position);

        IEnumerable<ScriptEntityDescription> AvailableEntities(ContentManager content);
    }
}
