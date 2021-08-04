#if UNITY_ANDROID
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    internal static class Converter
    {
        public static NativeNotificationType from(NotificationPermissionOptions options)
        {
            NotificationPermissionOptions compactOptions = options;

            NotificationPermissionOptions alertMask = NotificationPermissionOptions.Alert | NotificationPermissionOptions.CarPlay | NotificationPermissionOptions.CriticalAlert;
            if ((options & alertMask) != 0)
            {
                compactOptions = ((options & ~alertMask) | NotificationPermissionOptions.Alert); //Making sure we have only Alert set when all alert related options exist.
            }

            compactOptions = compactOptions & ~(NotificationPermissionOptions.Provisional | NotificationPermissionOptions.ProvidesAppNotificationSettings | NotificationPermissionOptions.Announcement);//Ignoring these settings to not get affected

            if (compactOptions == NotificationPermissionOptions.None)
            {
                return NativeNotificationType.None;
            }
            else if (compactOptions == NotificationPermissionOptions.Badge)
            {
                return NativeNotificationType.Badge;
            }
            else if(compactOptions == NotificationPermissionOptions.Sound)
            {
                return NativeNotificationType.Sound;
            }
            else if (compactOptions == NotificationPermissionOptions.Alert)
            {
                return NativeNotificationType.Alert;
            }
            else if (compactOptions == (NotificationPermissionOptions.Badge | NotificationPermissionOptions.Sound))
            {
                return NativeNotificationType.BadgeAndSound;
            }
            else if (compactOptions == (NotificationPermissionOptions.Badge | NotificationPermissionOptions.Alert))
            {
                return NativeNotificationType.AlertAndBadge;
            }
            else if (compactOptions == (NotificationPermissionOptions.Alert | NotificationPermissionOptions.Sound))
            {
                return NativeNotificationType.AlertAndSound;
            }
            else if (compactOptions == (NotificationPermissionOptions.Alert | NotificationPermissionOptions.Sound | NotificationPermissionOptions.Badge))
            {
                return NativeNotificationType.All;
            }
            else
            {
                DebugLogger.LogWarning("Defaults to Alert, badge and sound");
                return NativeNotificationType.All;
            }
        }

        public static NotificationPermissionOptions from(NativeNotificationType notificationType)
        {
            switch (notificationType)
            {
                case NativeNotificationType.None:
                    return NotificationPermissionOptions.None;
                case NativeNotificationType.Badge:
                    return NotificationPermissionOptions.Badge;
                case NativeNotificationType.Sound:
                    return NotificationPermissionOptions.Sound;
                case NativeNotificationType.Alert:
                    return NotificationPermissionOptions.Alert;
                case NativeNotificationType.AlertAndBadge:
                    return NotificationPermissionOptions.Alert | NotificationPermissionOptions.Badge;
                case NativeNotificationType.AlertAndSound:
                    return NotificationPermissionOptions.Alert | NotificationPermissionOptions.Sound;
                case NativeNotificationType.BadgeAndSound:
                    return NotificationPermissionOptions.Badge | NotificationPermissionOptions.Sound;
                case NativeNotificationType.All:
                    return NotificationPermissionOptions.Alert | NotificationPermissionOptions.Badge | NotificationPermissionOptions.Sound;
                default:
                    throw VBException.SwitchCaseNotImplemented(notificationType);
            }

        }

    }
}
#endif
