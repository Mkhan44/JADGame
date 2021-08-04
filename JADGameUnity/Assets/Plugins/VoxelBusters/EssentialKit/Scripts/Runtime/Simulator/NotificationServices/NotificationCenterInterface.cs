using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Simulator
{
    public sealed class NotificationCenterInterface : NativeNotificationCenterInterfaceBase, INativeNotificationCenterInterface
    {
        #region Constructors

        public NotificationCenterInterface() 
            : base(isAvailable: true)
        { }

        #endregion

        #region Base class methods

        public override void RequestPermission(NotificationPermissionOptions options, RequestPermissionInternalCallback callback)
        {
            NotificationServicesSimulator.Instance.RequestPermission(options, (status, error) =>
            {
                callback(status, error);
            });
        }

        public override void GetSettings(GetSettingsInternalCallback callback)
        {
            // send result
            var     enabledOptions  = NotificationPermissionOptions.None;
            var     settings        = new NotificationSettingsInternal(
                permissionStatus: NotificationServicesSimulator.Instance.GetPermissionStatus(out enabledOptions), 
                alertSetting: NotificationSettingStatus.NotSupported, 
                badgeSetting: NotificationSettingStatus.NotSupported,
                carPlaySetting: NotificationSettingStatus.NotSupported, 
                lockScreenSetting: NotificationSettingStatus.NotSupported, 
                notificationCenterSetting: NotificationSettingStatus.NotSupported, 
                soundSetting: NotificationSettingStatus.NotSupported, 
                criticalAlertSetting: NotificationSettingStatus.NotSupported, 
                announcementSetting: NotificationSettingStatus.NotSupported, 
                alertStyle: NotificationAlertStyle.None, 
                previewStyle: NotificationPreviewStyle.Never);
            callback(settings);
        }

        public override IMutableNotification CreateMutableNotification(string notificationId)
        {
            return new MutableNotification(notificationId);
        }

        public override void ScheduleNotification(INotification notification, ScheduleNotificationInternalCallback callback)
        {
            Error   error;
            NotificationServicesSimulator.Instance.AddNotification((Notification)notification, out error);

            // send result
            callback(error);
        }

        public override void GetScheduledNotifications(GetNotificationsInternalCallback callback)
        {
            Error   error;
            var     notifications   = NotificationServicesSimulator.Instance.GetScheduledNotifications(out error);

            // send result
            callback(notifications, error);
        }

        public override void CancelScheduledNotification(string notificationId)
        {
            // get scheduled notifications
            Error   error;
            var     notifications   = NotificationServicesSimulator.Instance.GetScheduledNotifications(out error);

            // find requested notification and cancel it
            var     notification    = Array.Find(notifications, (item) => string.Equals(notificationId, item.Id));
            if (notification != null)
            {
                NotificationServicesSimulator.Instance.RemoveScheduledNotification(notification);
            }
        }

        public override void CancelAllScheduledNotifications()
        {
            NotificationServicesSimulator.Instance.RemoveAllScheduledNotifications();
        }

        public override void GetDeliveredNotifications(GetNotificationsInternalCallback callback)
        {
            Error   error;
            var     notifications   = NotificationServicesSimulator.Instance.GetDeliveredNotifications(out error);

            // send result
            callback(notifications, error);
        }

        public override void RemoveAllDeliveredNotifications()
        {
            NotificationServicesSimulator.Instance.RemoveAllDeliveredNotifications();
        }

        public override void RegisterForPushNotifications(RegisterForPushNotificationsInternalCallback callback)
        {
            NotificationServicesSimulator.Instance.RegisterForRemoteNotification((deviceToken, error) => callback(deviceToken, error));
        }

        public override void UnregisterForPushNotifications()
        {
            NotificationServicesSimulator.Instance.UnregisterForRemoteNotification();
        }

        public override bool IsRegisteredForPushNotifications()
        { 
            return NotificationServicesSimulator.Instance.IsRegisteredForRemoteNotification();
        }

        public override void SetApplicationIconBadgeNumber(int count)
        {
            Diagnostics.LogNotSupportedInEditor("SetApplicationIconBadgeNumber in NotificationServices");
        }

        #endregion
    }
}