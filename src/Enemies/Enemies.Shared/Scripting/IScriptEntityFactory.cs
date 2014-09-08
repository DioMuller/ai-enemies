using Enemies.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Enemies.Scripting
{
    interface IScriptEntityFactory
    {
        BaseEntity LoadEntity(ContentManager content, string name, Vector2 position);

        IEnumerable<string> AvailableEntities(ContentManager content);
    }
}
