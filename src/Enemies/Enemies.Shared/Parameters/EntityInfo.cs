using Enemies.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Enemies.Parameters
{
    public class EntityInfo
    {
        public int Id { get; private set; }
        public TypeTag Tag { get; private set; }
        public Vector2 Position { get; private set; }

        public EntityInfo(int id, TypeTag tag, Vector2 position)
        {
            Id = id;
            Tag = tag;
            Position = position;
        }
    }
}
