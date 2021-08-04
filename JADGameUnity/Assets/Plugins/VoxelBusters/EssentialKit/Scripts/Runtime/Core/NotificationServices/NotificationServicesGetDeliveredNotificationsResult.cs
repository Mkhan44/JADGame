using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information retrieved when <see cref="NotificationServices.GetDeliveredNotifications(EventCallback{NotificationServicesGetDeliveredNotificationsResult})"/> request is completed.
    /// </summary>
    public class NotificationServicesGetDeliveredNotificationsResult
    {
        #region Properties

        /// <summary>
        /// An array of delivered notifications.
        /// </summary>
        public INotification[] Notifications
        {
            get;
            internal set;
        }

        #endregion
    }
}