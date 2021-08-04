using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides an interface to access an achievement's properties such as achievement's title, max points, image etc.
    /// </summary>
    /// <remarks>
    /// \note Your game must authenticate the local user before using any features.
    /// </remarks>
    public interface IAchievementDescription
    {
        #region Properties

        /// <summary>
        /// An unique string used to identify the achievement across all the supported platforms. (read-only)
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
        /// A localized title for the achievement. (read-only)
        /// </summary>
        string Title
        {
            get;
        }

        /// <summary>
        /// A localized description of the achievement to be used when the local player has not completed the achievement. (read-only)
        /// </summary>
        string UnachievedDescription
        {
            get;
        }

        /// <summary>
        /// A localized description to be used after the local player has completed the achievement. (read-only)
        /// </summary>
        string AchievedDescription
        {
            get;
        }

        /// <summary>
        /// The number of points the player earns by completing this achievement. (read-only)
        /// </summary>
        long MaximumPoints
        {
            get;
        }

        /// <summary>
        /// The number of steps required for completing this achievement.
        /// </summary>
        int NumberOfStepsRequiredToUnlockAchievement
        {
            get;
        }

        /// <summary>
        /// A boolean that states whether this achievement is initially visible to users. (read-only)
        /// </summary>
        bool IsHidden
        {
            get;
        }

        /// <summary>
        /// A Boolean value that states whether this achievement can be earned multiple times. (read-only)
        /// </summary>
        bool IsReplayable
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the image property for an incomplete achievement.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        void LoadIncompleteAchievementImage(EventCallback<TextureData> callback);

        /// <summary>
        /// Loads the image property for a completed achievement.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        void LoadImage(EventCallback<TextureData> callback);

        #endregion
    }
}