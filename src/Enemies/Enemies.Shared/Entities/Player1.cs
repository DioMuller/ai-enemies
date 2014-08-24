using Jv.Games.Xna.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Enemies.Entities
{
    class Player1 : Entity
    {
        public Player1(ContentManager content)
        {
            SpriteSheet spriteSheet = new SpriteSheet(content.Load<Texture2D>("sprites/Characters/main"), 9, 13);
            TimeSpan walkFrameDuration = TimeSpan.FromMilliseconds(100);

            Sprite.Add(spriteSheet.GetAnimation("stopped_up", line: 0, count: 1, frameDuration: walkFrameDuration));
            Sprite.Add(spriteSheet.GetAnimation("stopped_left", line: 1, count: 1, frameDuration: walkFrameDuration));
            Sprite.Add(spriteSheet.GetAnimation("stopped_down", line: 2, count: 1, frameDuration: walkFrameDuration));
            Sprite.Add(spriteSheet.GetAnimation("stopped_right", line: 3, count: 1, frameDuration: walkFrameDuration));

            Sprite.Add(spriteSheet.GetAnimation("walking_up", line: 0, count: 8, skipFrames: 1, frameDuration: walkFrameDuration));
            Sprite.Add(spriteSheet.GetAnimation("walking_left", line: 1, count: 8, skipFrames: 1, frameDuration: walkFrameDuration));
            Sprite.Add(spriteSheet.GetAnimation("walking_down", line: 2, count: 8, skipFrames: 1, frameDuration: walkFrameDuration));
            Sprite.Add(spriteSheet.GetAnimation("walking_right", line: 3, count: 8, skipFrames: 1, frameDuration: walkFrameDuration));

            Sprite.Add(spriteSheet.GetAnimation("dying", line: 12, count: 6, frameDuration: walkFrameDuration, repeat: false));

            Sprite.PlayAnimation("stopped_down");
        }
    }
}
