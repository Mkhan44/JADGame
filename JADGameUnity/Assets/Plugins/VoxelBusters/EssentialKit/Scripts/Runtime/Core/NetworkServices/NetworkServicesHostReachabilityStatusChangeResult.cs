using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This object contains the information retrieved when <see cref="NetworkServices.OnHostReachabilityChange"/> event occurs.
    /// </summary>
    public class NetworkServicesHostReachabilityStatusChangeResult
    {
        #region Properties

        /// <summary>
        /// This boolean value is used to determine whether host is reachable.
        /// </summary>
        /// <value><c>true</c> if is reachable; otherwise, <c>false</c>.</value>
        public bool IsReachable
        {
            get;
            internal set;
        }

        #endregion
    }
}