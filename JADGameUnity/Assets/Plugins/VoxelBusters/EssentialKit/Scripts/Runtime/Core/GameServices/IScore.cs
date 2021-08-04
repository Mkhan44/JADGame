using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides an interface to read the score that was earned by the user.
    /// </summary>
    /// <remarks>
    /// \note Your game must authenticate the local user before using any features.
    /// </remarks>
    public interface IScore
    {
        #region Properties

        /// <summary>
        /// An unique string used to identify the leaderboard across all the supported platforms. (read-only)
        /// </summary>
        string LeaderboardId
        {
            get;
        }

        /// <summary>
        /// A string used to identify the leaderboard in the current platform. (read-only)
        /// </summary>
        string LeaderboardPlatformId
        {
            get;
        }

        /// <summary>
        /// The player that earned the score. (read-only)
        /// </summary>
        IPlayer Player
        {
            get;
        }

        /// <summary>
        /// The position of the score in leaderboard. (read-only) 
        /// </summary>
        long Rank
        {
            get;
        }

        /// <summary>
        /// The score earned by the player.
        /// </summary>
        long Value
        {
            get;
            set;
        }

        /// <summary>
        /// The players score as a localized string. (read-only)
        /// </summary>
        string FormattedValue
        {
            get;
        }

        /// <summary>
        /// The date and time when score was reported. (read-only)
        /// </summary>
        DateTime LastReportedDate
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reports the score to game server.
        /// </summary>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        void ReportScore(CompletionCallback callback);

        #endregion
    }
}