using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="GameServices.LoadLeaderboards(EventCallback{GameServicesLoadLeaderboardsResult})"/> operation is completed.
    /// </summary>
    public class GameServicesLoadLeaderboardsResult
    {
        #region Properties

        /// <summary>
        /// An array of registered leaderboards.
        /// </summary>
        public ILeaderboard[] Leaderboards
        {
            get;
            internal set;
        }

        #endregion
    }
}