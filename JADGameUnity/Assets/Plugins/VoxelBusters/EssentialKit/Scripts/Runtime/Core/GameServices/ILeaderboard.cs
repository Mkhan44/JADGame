using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides interface to read data from a leaderboard stored on game server.
    /// </summary>
    /// <remarks>
    /// \note Your game must authenticate the local user before using any features.
    /// </remarks>
    public interface ILeaderboard
    {
        #region Properties

        /// <summary>
        /// An unique string used to identify the leaderboard across all the supported platforms. (read-only)
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// An unique used to identify the leaderboard in the current platform. (read-only)
        /// </summary>
        string PlatformId
        {
            get;
        }

        /// <summary>
        /// A localized title for the leaderboard. (read-only)
        /// </summary>
        string Title
        {
            get;
        }

        /// <summary>
        /// A filter used to restrict the search to a subset of the players on game server.
        /// </summary>
        LeaderboardPlayerScope PlayerScope
        {
            get;
            set;
        } 
            
        /// <summary>
        /// A filter used to restrict the search to scores that were posted within a specific period of time.
        /// </summary>
        LeaderboardTimeScope TimeScope
        {
            get;
            set;
        }

        /// <summary>
        /// The value indicates maximum entries that has to be fetched from search.
        /// </summary>
        int LoadScoresQuerySize
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the <see cref="IScore"/> earned by the local player. (read-only)
        /// </summary>
        /// <remarks>
        /// \note This property is invalid until a call to load scores is completed.
        /// </remarks>
        IScore LocalPlayerScore
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the top set of scores.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        void LoadTopScores(EventCallback<LeaderboardLoadScoresResult> callback);

        /// <summary>
        /// Loads player-centered set of scores.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        void LoadPlayerCenteredScores(EventCallback<LeaderboardLoadScoresResult> callback);

        /// <summary>
        /// Loads next set of scores.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        void LoadNext(EventCallback<LeaderboardLoadScoresResult> callback);

        /// <summary>
        /// Loads previous set of scores.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        void LoadPrevious(EventCallback<LeaderboardLoadScoresResult> callback);

        /// <summary>
        /// Loads the image.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        void LoadImage(EventCallback<TextureData> callback);

        #endregion
    }
}