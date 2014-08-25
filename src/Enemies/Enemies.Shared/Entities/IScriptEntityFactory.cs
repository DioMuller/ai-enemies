using Microsoft.Xna.Framework.Content;

namespace Enemies.Entities
{
    interface IScriptEntityFactory
    {
        Entity LoadEntity(ContentManager content, string name);
    }
}
