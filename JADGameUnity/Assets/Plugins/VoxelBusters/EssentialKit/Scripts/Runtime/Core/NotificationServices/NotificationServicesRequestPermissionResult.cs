using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="NotificationServices.RequestPermission(NotificationPermissionOptions, bool, EventCallback{NotificationServicesRequestPermissionResult})"/> operation is completed.
    /// </summary>
    public class NotificationServicesRequestPermissionResult
    {
        #region Properties

        /// <summary>
        /// The permission granted by the user.
        /// </summary>
        public NotificationPermissionStatus PermissionStatus
        {
            get;
            internal set;
        }

        #endregion
    }
}