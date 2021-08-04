#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{    
    [StructLayout(LayoutKind.Sequential)]
    internal struct UNNotificationSettingsData
    {
        #region Properties

        public UNAuthorizationStatus AuthorizationStatus
        {
            get;
            set;
        }

        public UNNotificationSetting AlertSetting
        {
            get;
            set;
        }

        public UNNotificationSetting BadgeSetting
        {
            get;
            set;
        }

        public UNNotificationSetting CarPlaySetting
        {
            get;
            set;
        }

        public UNNotificationSetting LockScreenSetting
        {
            get;
            set;
        }

        public UNNotificationSetting NotificationCenterSetting
        {
            get;
            set;
        }

        public UNNotificationSetting SoundSetting
        {
            get;
            set;
        }

        public UNNotificationSetting CriticalAlertSetting
        {
            get;
            set;
        }

        public UNNotificationSetting AnnouncementSetting
        {
            get;
            set;
        }

        public UNAlertStyle AlertStyle
        {
            get;
            set;
        }

        public UNShowPreviewsSetting ShowPreviewsSetting
        {
            get;
            set;
        }

        #endregion
    }
}
#endif