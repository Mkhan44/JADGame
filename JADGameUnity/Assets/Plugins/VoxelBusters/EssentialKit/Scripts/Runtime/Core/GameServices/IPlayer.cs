using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides a cross-platform interface to access information about a player playing your game.
    /// </summary>
    public interface IPlayer
    {
        #region Properties

        /// <summary>
        /// A string assigned by game service to uniquely identify a player. (read-only)
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// A string chosen by the player to identify themselves to others. (read-only)
        /// </summary>
        /// <description>
        /// This property is used when a player is not a friend of the local player. For displaying name on user interface, use the <see cref="DisplayName"/> property.
        /// </description>
        string Alias
        {
            get;
        }

        /// <summary>
        /// A string to display for the player. (read-only)
        /// </summary>
        /// <description>
        /// If the player is a friend of the local player, then the value returned is the actual name of the player. 
        /// And incase if he is not a friend, then players alias will be returned.
        /// </description>
        string DisplayName
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the player profile image.
        /// </summary>
        /// <param name="callback">Callback that will be called after operation is completed.</param>
        void LoadImage(EventCallback<TextureData> callback);

        #endregion
    }
}