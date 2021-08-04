using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when notification message is received.
    /// </summary>
    public class NotificationServicesNotificationReceivedResult
    {
        #region Properties

        /// <summary>
        /// The received notification.
        /// </summary>
        public INotification Notification
        {
            get;
            internal set;
        }

        #endregion
    }
}