using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NotificationServicesCore
{
    public interface INativeNotificationCenterInterface : INativeFeatureInterface
    {
        #region Events

        event NotificationReceivedInternalCallback OnNotificationReceived;

        #endregion

        #region Methods

        // settings methods
        void RequestPermission(NotificationPermissionOptions options, RequestPermissionInternalCallback callback);

        void GetSettings(GetSettingsInternalCallback callback);

        // local notification methods
        IMutableNotification CreateMutableNotification(string notificationId);

        void ScheduleNotification(INotification notification, ScheduleNotificationInternalCallback callback);

        void GetScheduledNotifications(GetNotificationsInternalCallback callback);

        void CancelScheduledNotification(string notificationId);

        void CancelAllScheduledNotifications();

        void GetDeliveredNotifications(GetNotificationsInternalCallback callback);

        void RemoveAllDeliveredNotifications();

        // remote notification methods
        void RegisterForPushNotifications(RegisterForPushNotificationsInternalCallback callback);

        void UnregisterForPushNotifications();

        bool IsRegisteredForPushNotifications();

        void SetApplicationIconBadgeNumber(int count);

        #endregion
    }
}