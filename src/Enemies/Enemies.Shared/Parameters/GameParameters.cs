using Enemies.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Enemies.Maps;
using Enemies.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Enemies.Parameters
{
    class GameParameters
    {
        /// <summary>
        /// Entity List
        /// </summary>
        public static EntityInfo[] Entities { get; private set; }

        public static Map CurrentMap { get; private set; }

        #region Methods
        /// <summary>
        /// Update current entities.
        /// </summary>
        /// <param name="entities">Current entity list.</param>
        public static void UpdateEntities(IEnumerable<IEntity> entities)
        {
            if (entities == null) Entities = null;
            else
            {
                Entities = entities.OfType<BaseEntity>().Select( (entity) => entity.GetInfo() ).ToArray();
            }
        }

        public static void LoadMap(GameScreen game, Point size, ContentManager content, string file)
        {
            //TODO: Change for different sizes?
            CurrentMap = new Map(game, size, content, file);
        }
        #endregion Methods
    }
}
