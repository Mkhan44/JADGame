using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NotificationServicesCore
{
    internal sealed class NullNotificationCenterInterface : NativeNotificationCenterInterfaceBase
    {
        #region Constructors

        public NullNotificationCenterInterface() 
            : base(isAvailable: false)
        { }

        #endregion

        #region Private static methods

        private static void LogNotSupported()
        {
            Diagnostics.LogNotSupported("NotificationServices");
        }

        #endregion

        #region Base class methods

        public override void RequestPermission(NotificationPermissionOptions options, RequestPermissionInternalCallback callback)
        {
            LogNotSupported();

            // send result
            callback(NotificationPermissionStatus.Denied, Diagnostics.kFeatureNotSupported);
        }

        public override void GetSettings(GetSettingsInternalCallback callback)
        {
            LogNotSupported();

            // send result
            var     settings    = new NotificationSettingsInternal(
                permissionStatus: NotificationPermissionStatus.Denied, 
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
            LogNotSupported();

            return new NullMutableNotification(notificationId);
        }

        public override void ScheduleNotification(INotification notification, ScheduleNotificationInternalCallback callback)
        {
            LogNotSupported();

            // send result
            callback(Diagnostics.kFeatureNotSupported);
        }

        public override void GetScheduledNotifications(GetNotificationsInternalCallback callback)
        {
            LogNotSupported();

            // send result
            callback(null, Diagnostics.kFeatureNotSupported);
        }

        public override void CancelScheduledNotification(string notificationId)
        {
            LogNotSupported();
        }

        public override void CancelAllScheduledNotifications()
        {
            LogNotSupported();
        }

        public override void GetDeliveredNotifications(GetNotificationsInternalCallback callback)
        {
            LogNotSupported();

            // send result
            callback(null, Diagnostics.kFeatureNotSupported);
        }

        public override void RemoveAllDeliveredNotifications()
        {
            LogNotSupported();
        }

        public override void RegisterForPushNotifications(RegisterForPushNotificationsInternalCallback callback)
        {
            LogNotSupported();

            // send result
            callback(null, Diagnostics.kFeatureNotSupported);
        }

        public override void UnregisterForPushNotifications()
        {
            LogNotSupported();
        }

        public override bool IsRegisteredForPushNotifications()
        { 
            return false;
        }

        public override void SetApplicationIconBadgeNumber(int count)
        {
            LogNotSupported();
        }

        #endregion
    }
}