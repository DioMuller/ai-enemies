using Enemies.Entities;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Enemies.Scripting
{
    interface IScriptEntityFactory
    {
        BaseEntity LoadEntity(ContentManager content, string name);

        IEnumerable<string> AvailableEntities(ContentManager content);
    }
}
