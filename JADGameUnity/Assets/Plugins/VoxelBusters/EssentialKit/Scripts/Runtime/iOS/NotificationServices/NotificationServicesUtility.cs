#if UNITY_IOS || UNITY_TVOS
using System;
using System.Runtime.InteropServices;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{
    internal static class NotificationServicesUtility 
    {
        #region Convert methods

        public static NotificationPermissionStatus ConvertToNotificationPermissionStatus(UNAuthorizationStatus status)
        {
            switch (status)
            {
                case UNAuthorizationStatus.UNAuthorizationStatusNotDetermined:
                    return NotificationPermissionStatus.NotDetermined;

                case UNAuthorizationStatus.UNAuthorizationStatusProvisional:
                    return NotificationPermissionStatus.Provisional;

                case UNAuthorizationStatus.UNAuthorizationStatusDenied:
                    return NotificationPermissionStatus.Denied;

                case UNAuthorizationStatus.UNAuthorizationStatusAuthorized:
                    return NotificationPermissionStatus.Authorized;

                default:
                    throw VBException.SwitchCaseNotImplemented(status);
            }
        }

        public static UNAuthorizationOptions ConvertToUNAuthorizationOptions(NotificationPermissionOptions unityOptions)
        {
            var     nativeOptions   = (UNAuthorizationOptions)0;
            if (unityOptions.Contains(NotificationPermissionOptions.Badge))
            {
                nativeOptions |= UNAuthorizationOptions.UNAuthorizationOptionBadge;
            }
            if (unityOptions.Contains(NotificationPermissionOptions.Alert))
            {
                nativeOptions |= UNAuthorizationOptions.UNAuthorizationOptionAlert;
            }
            if (unityOptions.Contains(NotificationPermissionOptions.Sound))
            {
                nativeOptions |= UNAuthorizationOptions.UNAuthorizationOptionSound;
            }
            if (unityOptions.Contains(NotificationPermissionOptions.CarPlay))
            {
                nativeOptions |= UNAuthorizationOptions.UNAuthorizationOptionCarPlay;
            }
            if (unityOptions.Contains(NotificationPermissionOptions.CriticalAlert))
            {
                nativeOptions |= UNAuthorizationOptions.UNAuthorizationOptionCriticalAlert;
            }
            if (unityOptions.Contains(NotificationPermissionOptions.Provisional))
            {
                nativeOptions |= UNAuthorizationOptions.UNAuthorizationOptionProvisional;
            }
            if (unityOptions.Contains(NotificationPermissionOptions.ProvidesAppNotificationSettings))
            {
                nativeOptions |= UNAuthorizationOptions.UNAuthorizationOptionProvidesAppNotificationSettings;
            }
            if (unityOptions.Contains(NotificationPermissionOptions.Announcement))
            {
                nativeOptions |= UNAuthorizationOptions.UNAuthorizationOptionAnnouncement;
            }

            return nativeOptions;
        }

        public static NotificationSettingStatus ConvertToNotificationSettingStatus(UNNotificationSetting status)
        {
            switch (status)
            {
                case UNNotificationSetting.UNNotificationSettingDisabled:
                    return NotificationSettingStatus.Disabled;

                case UNNotificationSetting.UNNotificationSettingEnabled:
                    return NotificationSettingStatus.Enabled;

                case UNNotificationSetting.UNNotificationSettingNotSupported:
                    return NotificationSettingStatus.NotSupported;

                default:
                    throw VBException.SwitchCaseNotImplemented(status);
            }
        }

        public static NotificationPreviewStyle ConvertToNotificationPreviewStyle(UNShowPreviewsSetting style)
        {
            switch (style)
            {
                case UNShowPreviewsSetting.UNShowPreviewsSettingAlways:
                    return NotificationPreviewStyle.Always;

                case UNShowPreviewsSetting.UNShowPreviewsSettingNever:
                    return NotificationPreviewStyle.Never;

                case UNShowPreviewsSetting.UNShowPreviewsSettingWhenAuthenticated:
                    return NotificationPreviewStyle.WhenAuthenticated;

                default:
                    throw VBException.SwitchCaseNotImplemented(style);
            }
        }

        public static NotificationAlertStyle ConvertToNotificationAlertStyle(UNAlertStyle style)
        {
            switch (style)
            {
                case UNAlertStyle.UNAlertStyleAlert:
                    return NotificationAlertStyle.Alert;

                case UNAlertStyle.UNAlertStyleBanner:
                    return NotificationAlertStyle.Banner;

                case UNAlertStyle.UNAlertStyleNone:
                    return NotificationAlertStyle.None;

                default:
                    throw VBException.SwitchCaseNotImplemented(style);
            }
        }

        public static UNNotificationPresentationOptions ConvertToUNNotificationPresentationOptions(NotificationPresentationOptions unityOptions)
        {
            var     nativeOptions    = (UNNotificationPresentationOptions)0;
            if (unityOptions.Contains(NotificationPresentationOptions.Alert))
            {
                nativeOptions |= UNNotificationPresentationOptions.UNNotificationPresentationOptionAlert;
            }
            if (unityOptions.Contains(NotificationPresentationOptions.Badge))
            {
                nativeOptions |= UNNotificationPresentationOptions.UNNotificationPresentationOptionBadge;
            }
            if (unityOptions.Contains(NotificationPresentationOptions.Sound))
            {
                nativeOptions |= UNNotificationPresentationOptions.UNNotificationPresentationOptionSound;
            }

            return nativeOptions;
        }

        public static bool Contains(this UNNotificationSetting options, UNNotificationSetting value)
        {
            return (options & value) != 0;
        }

        public static bool Contains(this NotificationPermissionOptions options, NotificationPermissionOptions value)
        {
            return (options & value) != 0;
        }

        public static bool Contains(this NotificationPresentationOptions options, NotificationPresentationOptions value)
        {
            return (options & value) != 0;
        }

        #endregion
    }
}
#endif