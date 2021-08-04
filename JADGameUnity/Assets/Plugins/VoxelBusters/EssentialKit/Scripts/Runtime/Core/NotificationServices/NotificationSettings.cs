using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    public class NotificationSettings
    {
        #region Properties

        /// <summary>
        /// The permission granted by the user.
        /// </summary>
        public NotificationPermissionStatus PermissionStatus
        {
            get;
            private set;
        }

        /// <summary>
        /// The authorization status for displaying alerts.
        /// </summary>
        public NotificationSettingStatus AlertSetting
        {
            get;
            private set;
        }

        /// <summary>
        /// The setting that indicates whether badges appear on your app’s icon.
        /// </summary>
        public NotificationSettingStatus BadgeSetting
        {
            get;
            private set;
        }

        /// <summary>
        /// The setting that indicates whether your app’s notifications appear in CarPlay.
        /// </summary>
        public NotificationSettingStatus CarPlaySetting
        {
            get;
            private set;
        }

        /// <summary>
        /// The setting that indicates whether your app’s notifications appear on a device’s Lock screen.
        /// </summary>
        public NotificationSettingStatus LockScreenSetting
        {
            get;
            private set;
        }

        /// <summary>
        /// The setting that indicates whether your app’s notifications appear in Notification Center.
        /// </summary>
        public NotificationSettingStatus NotificationCenterSetting
        {
            get;
            private set;
        }

        /// <summary>
        /// The authorization status for playing sounds for incoming notifications.
        /// </summary>
        public NotificationSettingStatus SoundSetting
        {
            get;
            private set;
        }

        /// <summary>
        /// The authorization status for playing sounds for critical alerts.
        /// </summary>
        public NotificationSettingStatus CriticalAlertSetting
        {
            get;
            private set;
        }

        /// <summary>
        /// The setting that indicates whether Siri can announce your app’s notifications.
        /// </summary>
        public NotificationSettingStatus AnnouncementSetting
        {
            get;
            private set;
        }

        /// <summary>
        /// The type of alert that the app may display when the device is unlocked.
        /// </summary>
        public NotificationAlertStyle AlertStyle
        {
            get;
            private set;
        }

        /// <summary>
        /// The setting that indicates whether the app shows a preview of the notification's content.
        /// </summary>
        public NotificationPreviewStyle PreviewStyle
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether push notification is enabled.
        /// </summary>
        public bool PushNotificationEnabled
        {
            get;
            private set;
        }

        /// <summary>
        /// The device token received while registering for remote notification service.
        /// </summary>
        public string DeviceToken
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        internal NotificationSettings(NotificationPermissionStatus permissionStatus, NotificationSettingStatus alertSetting,
            NotificationSettingStatus badgeSetting, NotificationSettingStatus carPlaySetting, 
            NotificationSettingStatus lockScreenSetting, NotificationSettingStatus notificationCenterSetting,
            NotificationSettingStatus soundSetting, NotificationSettingStatus criticalAlertSetting, 
            NotificationSettingStatus announcementSetting, NotificationAlertStyle alertStyle, 
            NotificationPreviewStyle previewStyle, bool pushNotificationEnabled, string deviceToken)
        {
            // set properties
            PermissionStatus            = permissionStatus;
            AlertSetting                = alertSetting;
            BadgeSetting                = badgeSetting;
            CarPlaySetting              = carPlaySetting;
            LockScreenSetting           = lockScreenSetting;
            NotificationCenterSetting   = notificationCenterSetting;
            SoundSetting                = soundSetting;
            CriticalAlertSetting        = criticalAlertSetting;
            AnnouncementSetting         = announcementSetting;
            AlertStyle                  = alertStyle;
            PreviewStyle                = previewStyle;
            PushNotificationEnabled     = pushNotificationEnabled;
            DeviceToken                 = deviceToken;
        }

        #endregion

        #region Base class methods

        public override string ToString()
        {
            var     sb  = new StringBuilder();
            sb.Append("NotificationSettings {");
            sb.Append("PermissionStatus: ").Append(PermissionStatus).Append(" ");
            sb.Append("AlertSetting: ").Append(AlertSetting).Append(" ");
            sb.Append("BadgeSetting: ").Append(BadgeSetting).Append(" ");
            sb.Append("CarPlaySetting: ").Append(CarPlaySetting).Append(" ");
            sb.Append("LockScreenSetting: ").Append(LockScreenSetting).Append(" ");
            sb.Append("NotificationCenterSetting: ").Append(NotificationCenterSetting).Append(" ");
            sb.Append("SoundSetting: ").Append(SoundSetting).Append(" ");
            sb.Append("CriticalAlertSetting: ").Append(CriticalAlertSetting).Append(" ");
            sb.Append("AnnouncementSetting: ").Append(AnnouncementSetting).Append(" ");
            sb.Append("AlertStyle: ").Append(AlertStyle).Append(" ");
            sb.Append("PreviewStyle: ").Append(PreviewStyle).Append(" ");
            sb.Append("PushNotificationEnabled: ").Append(PushNotificationEnabled).Append(" ");
            sb.Append("DeviceToken: ").Append(DeviceToken);
            sb.Append("}");
            return sb.ToString();

        }
        #endregion
    }
}