using System;
using Microsoft.Xna.Framework;

namespace Enemies.Behaviors
{
    public interface IBehavior
    {
        bool Enabled { get; }
        void Update(GameTime gameTime);
    }
}

