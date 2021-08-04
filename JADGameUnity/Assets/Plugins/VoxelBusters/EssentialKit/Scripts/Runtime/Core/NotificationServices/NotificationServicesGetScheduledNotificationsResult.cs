using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="NotificationServices.GetScheduledNotifications(EventCallback{NotificationServicesGetScheduledNotificationsResult})"/> request is completed.
    /// </summary>
    public class NotificationServicesGetScheduledNotificationsResult
    {
        #region Properties

        /// <summary>
        /// The scheduled notifications.
        /// </summary>
        public INotification[] Notifications
        {
            get;
            internal set;
        }

        #endregion
    }
}