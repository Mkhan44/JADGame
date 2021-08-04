using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides an interface to communicate with game server about local players progress towards completing achievement.
    /// </summary>
    /// <description> 
    /// By default, when an achievement is completed, a notification banner is displayed to the player. 
    /// If your game wants to display its own interface, you can prevent it by setting <b>ShowAchievementCompletionBanner</b> property in <b>Game Services Settings</b> to <b>NO</b>.
    /// </description>
    /// <remarks>
    /// \note Your game must authenticate the local user before using any features.
    /// </remarks>
    public interface IAchievement
    {
        #region Properties

        /// <summary>
        /// An unique identifier used to identify the achievement across all the supported platforms. (read-only)
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// A string used to identify the achievement in the current platform. (read-only)
        /// </summary>
        string PlatformId
        {
            get;
        }

        /// <summary>
        /// The percentage describes how far the player has progressed on this achievement.
        /// </summary>
        double PercentageCompleted
        {
            get;
            set;
        }

        /// <summary>
        /// The bool value indicates whether the current player has completed this achievement. (read-only)
        /// </summary>
        bool IsCompleted
        {
            get;
        }

        /// <summary>
        /// The last time that progress on the achievement was successfully reported to game server. (read-only)
        /// </summary>
        DateTime LastReportedDate
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reports the progress of this achievement.
        /// </summary>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        void ReportProgress(CompletionCallback callback);

        #endregion
    }
}