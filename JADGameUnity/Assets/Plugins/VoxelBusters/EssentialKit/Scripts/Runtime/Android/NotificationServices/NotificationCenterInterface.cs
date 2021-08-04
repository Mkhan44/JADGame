#if UNITY_ANDROID
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    public sealed class NotificationCenterInterface : NativeNotificationCenterInterfaceBase, INativeNotificationCenterInterface
    {
        #region Fields

        private NativeNotificationServices m_instance = null;
        private NotificationPermissionOptions m_options;

        #endregion

        #region Constructors

        public NotificationCenterInterface()
            : base(isAvailable: true)
        {
            m_instance = new NativeNotificationServices(NativeUnityPluginUtility.GetContext());
            m_instance.SetNotificationListener(new NativeNotificationReceivedListener()
            {
                onNotificationReceivedCallback = (NativeNotification nativeNotification) =>
                {
                    DebugLogger.Log("Received Notification");
                    SendNotificationReceivedEvent(new Notification(nativeNotification));
                }
            });
        }

        #endregion

        #region Base class methods

        public override void RequestPermission(NotificationPermissionOptions options, RequestPermissionInternalCallback callback)
        {
            this.m_options = options;
            m_instance.SetNotificationType(Converter.from(options));

            bool allowedByUser = m_instance.AreNotificationsAllowedByUser();
            callback(allowedByUser ? NotificationPermissionStatus.Authorized : NotificationPermissionStatus.Denied, null);
        }

        public override void GetSettings(GetSettingsInternalCallback callback)
        {
            bool allowedByUser = m_instance.AreNotificationsAllowedByUser();
            bool arePermissionsUnknown = (m_instance.GetNotificationType() == NativeNotificationType.Unknown);
            bool areAlertsAllowed = (m_options & (NotificationPermissionOptions.Alert | NotificationPermissionOptions.CarPlay | NotificationPermissionOptions.ProvidesAppNotificationSettings | NotificationPermissionOptions.Provisional | NotificationPermissionOptions.CriticalAlert | NotificationPermissionOptions.Announcement)) != 0;
            NotificationSettingsInternal settings = new NotificationSettingsInternal(
                permissionStatus: arePermissionsUnknown ? NotificationPermissionStatus.NotDetermined : allowedByUser ? NotificationPermissionStatus.Authorized : NotificationPermissionStatus.Denied,
                alertSetting: areAlertsAllowed ? NotificationSettingStatus.Enabled : NotificationSettingStatus.Disabled,
                badgeSetting: NotificationSettingStatus.NotAccessible,
                carPlaySetting: NotificationSettingStatus.NotSupported,
                lockScreenSetting: NotificationSettingStatus.NotAccessible,
                notificationCenterSetting: NotificationSettingStatus.Enabled,
                soundSetting: NotificationSettingStatus.NotAccessible,//m_instance.AreSoundsEnabledByUser() ? NotificationSettingStatus.Enabled : NotificationSettingStatus.Disabled,
                criticalAlertSetting: NotificationSettingStatus.NotSupported,
                announcementSetting: NotificationSettingStatus.NotSupported,
                alertStyle: NotificationAlertStyle.Banner,
                previewStyle: NotificationPreviewStyle.NotAccessible
            );

            callback(settings);
        }

        public override IMutableNotification CreateMutableNotification(string notificationId)
        {
            return new MutableNotification(notificationId);
        }

        public override void ScheduleNotification(INotification notification, ScheduleNotificationInternalCallback callback)
        {
            MutableNotification mutableNotification = (MutableNotification)notification;
            NativeNotification nativeNotification = mutableNotification.Build();
            m_instance.ScheduleNotification(nativeNotification, new NativeScheduleNotificationListener()
            {
                onSuccessCallback = () => callback(null),
                onFailureCallback = (error) => callback(new Error(error))
            });
        }

        public override void GetScheduledNotifications(GetNotificationsInternalCallback callback)
        {
            m_instance.RequestScheduledNotifications(new NativeNotificationsRequestListener()
            {
                onSuccessCallback = (nativeNotifications) =>
                {
                    Notification[] notifications = NativeUnityPluginUtility.Map<NativeNotification, Notification>(nativeNotifications.Get());
                    callback(notifications, null);
                },
                onFailureCallback = (error) => callback(null, new Error(error))
            });
        }

        public override void CancelScheduledNotification(string notificationId)
        {
            m_instance.CancelScheduledNotification(notificationId);
        }

        public override void CancelAllScheduledNotifications()
        {
            m_instance.CancelAllScheduledNotifications();
        }

        public override void GetDeliveredNotifications(GetNotificationsInternalCallback callback)
        {
            m_instance.RequestActiveNotifications(new NativeNotificationsRequestListener()
            {
                onSuccessCallback = (nativeNotifications) =>
                {
                    Notification[] notifications = NativeUnityPluginUtility.Map<NativeNotification, Notification>(nativeNotifications.Get());
                    callback(notifications, null);
                },
                onFailureCallback = (error) => callback(null, new Error(error))
            });
        }

        public override void RemoveAllDeliveredNotifications()
        {
            m_instance.ClearAllActiveNotifications();
        }

        public override void RegisterForPushNotifications(RegisterForPushNotificationsInternalCallback callback)
        {
            m_instance.RegisterRemoteNotifications(new NativeRegisterRemoteNotificationsListener()
            {
                onSuccessCallback = (token) => callback(token, null),
                onFailureCallback = (error) => callback(null, new Error(error))
            });
        }

        public override void UnregisterForPushNotifications()
        {
            m_instance.UnregisterRemoteNotifications(null);
        }

        public override bool IsRegisteredForPushNotifications()
        {
            return m_instance.AreRemoteNotificationsRegistered();
        }

        public override void SetApplicationIconBadgeNumber(int count)
        {
            //m_instance.SetApplicationIconBadgeNumber(count);
        }

        #endregion
    }
}
#endif