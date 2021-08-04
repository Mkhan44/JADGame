using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides interface to access information about the authenticated player running your game on the device. 
    /// </summary>
    /// <description>
    /// At any given time, only one user may be authenticated on the device, this user must log out before another user can log in.
    /// </description>
    /// <remarks>
    /// \note Your game must authenticate the local user before using any features.
    /// </remarks>
    public interface ILocalPlayer : IPlayer
    {
        #region Properties

        /// <summary>
        /// A bool value that indicates whether a local player is currently signed in to game service. (read-only)
        /// </summary>
        bool IsAuthenticated
        {
            get;
        }

        /// <summary>
        /// A bool value that indicates whether a local player is underage. (read-only)
        /// </summary>
        /// <value><c>true</c> if is under age; otherwise, <c>false</c>.</value>
        bool IsUnderAge
        {
            get;
        }

        #endregion
    }
}