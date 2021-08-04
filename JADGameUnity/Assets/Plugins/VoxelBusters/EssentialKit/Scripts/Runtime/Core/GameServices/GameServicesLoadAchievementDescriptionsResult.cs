using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="GameServices.LoadAchievements(EventCallback{GameServicesLoadAchievementsResult})"/> operation is completed.
    /// </summary>
    public class GameServicesLoadAchievementDescriptionsResult
    {
        #region Properties

        /// <summary>
        /// An array of achievement descriptions.
        /// </summary>
        public IAchievementDescription[] AchievementDescriptions
        {
            get;
            internal set;
        }

        #endregion
    }
}
