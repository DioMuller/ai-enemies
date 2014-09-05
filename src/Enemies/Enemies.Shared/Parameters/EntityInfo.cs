using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enemies.Parameters
{
    public class EntityInfo
    {
        public string Tag { get; private set; }
        public Vector2 Position { get; private set; }

        public EntityInfo(string tag, Vector2 position)
        {
            Tag = tag;
            Position = position;
        }
    }
}
