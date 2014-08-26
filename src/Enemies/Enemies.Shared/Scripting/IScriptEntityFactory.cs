using Enemies.Entities;
using Microsoft.Xna.Framework.Content;

namespace Enemies.Scripting
{
    interface IScriptEntityFactory
    {
        Entity LoadEntity(ContentManager content, string name);
    }
}
