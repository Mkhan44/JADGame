using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="GameServices.Authenticate"/> operation is completed.
    /// </summary>
    public class GameServicesAuthStatusChangeResult
    {
        #region Properties

        /// <summary>
        /// The local player.
        /// </summary>
        public ILocalPlayer LocalPlayer
        {
            get;
            internal set;
        }

        /// <summary>
        /// The value is used to determine whether user is logged in to system.
        /// </summary>
        public LocalPlayerAuthStatus AuthStatus
        {
            get;
            internal set;
        }

        #endregion
    }
}
