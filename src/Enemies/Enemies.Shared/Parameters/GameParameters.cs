using Enemies.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Enemies.Parameters
{
    class GameParameters
    {
        /// <summary>
        /// Entity List
        /// </summary>
        public static EntityInfo[] Entities { get; private set; }

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
        #endregion Methods
    }
}
