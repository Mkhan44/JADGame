using System;
using System.Collections;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit.NotificationServicesCore;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides cross-platform interface for scheduling, registering and handling notifications.
    /// </summary>
    public static class NotificationServices
    {
        #region Static fields

        private     static      INativeNotificationCenterInterface      s_nativeInterface       = null;

        private     static      string                                  s_deviceToken           = null;
        
        #endregion

        #region Static properties

        public static NotificationServicesUnitySettings UnitySettings
        {
            get
            {
                return EssentialKitSettings.Instance.NotificationServicesSettings;
            }
        }

        internal static INativeNotificationCenterInterface NativeInterface
        {
            get
            {
                return s_nativeInterface;
            }
        }

        /// <summary>
        /// Returns the cached scheduled notification array.
        /// </summary>
        /// <remarks>
        /// \note This property is invalid until a call to <see cref="GetScheduledNotifications(EventCallback{NotificationServicesGetScheduledNotificationsResult})"/> is completed.
        /// </remarks>
        public static INotification[] ScheduledNotifications
        {
            get;
            private set;
        }

        public static NotificationSettings CachedSettings 
        { 
            get; 
            private set;
        }

        #endregion

        #region Static events

        public static event Callback<NotificationSettings> OnSettingsUpdate;

        public static event Callback<NotificationServicesNotificationReceivedResult> OnNotificationReceived;

        public static event EventCallback<NotificationServicesRegisterForPushNotificationsResult> OnRegisterForPushNotificationsComplete;

        #endregion

        #region Static methods

        public static bool IsAvailable()
        {
            return (s_nativeInterface != null) && s_nativeInterface.IsAvailable;
        }

        internal static void Initialize()
        {
            // create interface object
            s_nativeInterface       = NativeFeatureActivator.CreateInterface<INativeNotificationCenterInterface>(ImplementationBlueprint.NotificationServices, UnitySettings.IsEnabled);
            RegisterForEvents();
            
            // set default values
            ScheduledNotifications  = new INotification[0];
        }

        /// <summary>
        /// Requests for permission to interact with the user when local and remote notifications are delivered to the user's device.
        /// </summary>
        /// <param name="options">The authorization options your app is requesting. You may combine the available constants to request authorization for multiple items.</param>
        /// <param name="showPrepermissionDialog">Indicates whether pre-confirmation is required, before prompting system permission dialog.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void RequestPermission(NotificationPermissionOptions options, bool showPrepermissionDialog = true, EventCallback<NotificationServicesRequestPermissionResult> callback = null)
        {
            // check whether preconfirmation dialog needs to be shown before triggering system permission dialog
            var     permissionHandler   = NativeFeatureUsagePermissionHandler.Default;
            if (showPrepermissionDialog && (permissionHandler != null))
            {
                // start request on receiving user permission
                permissionHandler.ShowPrepermissionDialog(
                    permissionType: NativeFeatureUsagePermissionType.kNotification,
                    onAllowCallback: () =>
                    {
                        RequestPermissionInternal(options, callback);
                    },
                    onDenyCallback: () =>
                    {
                        var     result  = new NotificationServicesRequestPermissionResult() { PermissionStatus = NotificationPermissionStatus.NotDetermined };
                        CallbackDispatcher.InvokeOnMainThread(callback, result, new Error(description: "User cancelled."));
                    });
            }
            else
            {
                RequestPermissionInternal(options, callback);
            }
        }

        /// <summary>
        /// Gets the notification settings available for this application.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void GetSettings(Callback<NotificationServicesGetSettingsResult> callback = null)
        {
            GetSettingsInternal(sendUpdateEvent: true, callback: callback);
        }

        #endregion

        #region Local notification methods

        /// <summary>
        /// Creates a new instance of local notification.
        /// </summary>
        /// <returns>The notification.</returns>
        /// <param name="notificationId">Notification identifier.</param>
        public static NotificationBuilder CreateNotificationWithId(string notificationId)
        {
            try
            {
                // create new object
                return NotificationBuilder.CreateNotification(notificationId);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return null;
            }
        }

        /// <summary>
        /// Schedules a local notification for delivery.
        /// </summary>
        /// <param name="notification">Notification.</param>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void ScheduleNotification(INotification notification, CompletionCallback callback = null)
        {
            // validate arguments
            if (null == notification)
            {
                DebugLogger.LogError("Notification object is null.");
                return;
            }

            try
            {
                // make request
                s_nativeInterface.ScheduleNotification(notification, (error) =>
                {
                    // send result to caller object
                    CallbackDispatcher.InvokeOnMainThread(callback, error);
                });
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Returns a list of all notification requests that are scheduled and waiting to be delivered.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void GetScheduledNotifications(EventCallback<NotificationServicesGetScheduledNotificationsResult> callback = null)
        {
            try
            {
                // make native request
                s_nativeInterface.GetScheduledNotifications((notifications, error) =>
                {
                    // avoid passing null value
                    notifications           = notifications ?? new INotification[0];                

                    // cache value
                    ScheduledNotifications  = notifications;

                    // send result to caller object
                    var     result          = new NotificationServicesGetScheduledNotificationsResult()
                    {
                        Notifications       = notifications,
                    };
                    CallbackDispatcher.InvokeOnMainThread(callback, result, error);
                });
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Unschedules the specified notification.
        /// </summary>
        /// <param name="notificationId">Notification id.</param>
        public static void CancelScheduledNotification(string notificationId)
        { 
            // validate arguments
            if (string.IsNullOrEmpty(notificationId))
            {
                DebugLogger.LogError("Notification id is null/empty.");
                return;
            }

            try
            {
                // make request
                s_nativeInterface.CancelScheduledNotification(notificationId);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Unschedules the specified notification.
        /// </summary>
        /// <param name="notification">Notification.</param>
        public static void CancelScheduledNotification(INotification notification)
        { 
            // validate arguments
            if (null == notification)
            {
                DebugLogger.LogError("Notification object is null.");
                return;
            }

            try
            {
                // make request
                s_nativeInterface.CancelScheduledNotification(notification.Id);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Unschedules all pending notification requests.
        /// </summary>
        public static void CancelAllScheduledNotifications()
        {
            try
            {
                // make request
                s_nativeInterface.CancelAllScheduledNotifications();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Returns a list of the app’s notifications that are still displayed in Notification Center.
        /// </summary>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void GetDeliveredNotifications(EventCallback<NotificationServicesGetDeliveredNotificationsResult> callback = null)
        {
            try
            {
                // make request
                s_nativeInterface.GetDeliveredNotifications((notifications, error) =>
                {
                    // avoid passing null
                    notifications       = notifications ?? new INotification[0];  

                    // send result to caller object
                    var     result      = new NotificationServicesGetDeliveredNotificationsResult()
                    {
                        Notifications   = notifications,
                    };
                    CallbackDispatcher.InvokeOnMainThread(callback, result, error);
                });
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Removes all of the app’s delivered notifications from Notification Center.
        /// </summary>
        public static void RemoveAllDeliveredNotifications()
        {
            try
            {
                // make request
                s_nativeInterface.RemoveAllDeliveredNotifications();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        #endregion

        #region Remote notification methods

        /// <summary>
        /// Registers to receive remote notifications via Push Notification service.
        /// </summary>
        /// <description>
        /// Call this method to initiate the registration process with Push Notification service. 
        /// When registration process completes, callback is fired.
        /// If registration succeeds, then you should pass device token to the server you use to generate remote notifications.
        /// </description>
        /// <remarks>
        /// \note If you want your app’s remote notifications to display alerts, play sounds etc you must call the <see cref="RequestPermission(NotificationPermissionOptions, bool, EventCallback{NotificationServicesRequestPermissionResult})"/> method before registering for remote notifications.
        /// </remarks>
        /// <param name="callback">Callback method that will be invoked after operation is completed.</param>
        public static void RegisterForPushNotifications(EventCallback<NotificationServicesRegisterForPushNotificationsResult> callback = null)
        {
            try
            {
                // make request
                s_nativeInterface.RegisterForPushNotifications((deviceToken, error) =>
                {
                    // cache device token 
                    if (null == error)
                    {
                        s_deviceToken   = deviceToken;
                        CopyPushNotificationPropertiesToCachedSettings();
                    }

                    // notify listeners
                    var     result      = new NotificationServicesRegisterForPushNotificationsResult()
                    {
                        DeviceToken     = deviceToken,
                    };
                    CallbackDispatcher.InvokeOnMainThread(callback, result, error);
                    CallbackDispatcher.InvokeOnMainThread(OnRegisterForPushNotificationsComplete, result, error);
                });
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Unregister for all remote notifications received via Push Notification service.
        /// </summary>
        /// <remarks>
        /// \note Apps unregistered through this method can always re-register.
        /// </remarks>
        public static void UnregisterForPushNotifications()
        {
            try
            {
                // make request
                s_nativeInterface.UnregisterForPushNotifications();

                // unset value
                s_deviceToken   = null;
                CopyPushNotificationPropertiesToCachedSettings();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        /// <summary>
        /// Returns the registeration status for remote notifications.
        /// </summary>
        /// <returns><c>true</c>, if registered for remote notifications, <c>false</c> otherwise.</returns>
        public static bool IsRegisteredForPushNotifications()
        {
            try
            {
                // make request
                return s_nativeInterface.IsRegisteredForPushNotifications();
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
                return false;
            }
        }

        /// <summary>
        /// Set application icon badge number.
        /// <remarks>
        /// \note When set to 0, the application badge will be cleared from the icon.
        /// </remarks>
        public static void SetApplicationIconBadgeNumber(int count)
        {
            try
            {
                // make request
                s_nativeInterface.SetApplicationIconBadgeNumber(count);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        #endregion

        #region Helper methods

        public static bool IsAuthorizedPermissionStatus(NotificationPermissionStatus accessStatus)
        {
            return (NotificationPermissionStatus.Authorized == accessStatus) ||
                (NotificationPermissionStatus.Provisional == accessStatus);
        }

        public static bool? IsInitializedAndAuthorized()
        {
            return (CachedSettings == null) 
                ? (bool?) null
                : IsAuthorizedPermissionStatus(CachedSettings.PermissionStatus);
        }

        public static bool IsAuthorized()
        {
            return (CachedSettings != null) && IsAuthorizedPermissionStatus(CachedSettings.PermissionStatus);
        }

        public static bool IsPermissionAvailable()
        {
            return (CachedSettings != null) && (CachedSettings.PermissionStatus != NotificationPermissionStatus.NotDetermined);
        }

        // returns device token if user has already registered with the token
        public static void TryRegisterForPushNotifications()
        {
            // check device settings whether app has required permissions
            var     authorized  = IsInitializedAndAuthorized();
            if (authorized == null)
            {
                GetSettings((result) =>
                {
                    if (IsAuthorizedPermissionStatus(result.Settings.PermissionStatus))
                    {
                        // initiate registration request
                        RegisterForPushNotifications();
                    }
                });
            }
            else if (authorized.Value)
            {
                // initiate registration request
                RegisterForPushNotifications();
            }
        }

        #endregion

        #region Private methods

        private static void RegisterForEvents()
        {
            s_nativeInterface.OnNotificationReceived    += HandleNotificationReceivedInternalCallback;
        }

        private static void UnregisterFromEvents()
        {
            s_nativeInterface.OnNotificationReceived    -= HandleNotificationReceivedInternalCallback;
        }

        private static void RequestPermissionInternal(NotificationPermissionOptions options, EventCallback<NotificationServicesRequestPermissionResult> callback = null)
        {
            try
            {
                // make request
                s_nativeInterface.RequestPermission(options, (permissionStatus, accessError) =>
                {
                    // update settings information
                    GetSettingsInternal(sendUpdateEvent: true, callback: (settingsResult) =>
                    {
                        // register for push notification if required
                        if (IsAuthorizedPermissionStatus(permissionStatus) && (UnitySettings.PushNotificationServiceType != PushNotificationServiceType.None))
                        {
                            // register for remote notification
                            RegisterForPushNotifications((result, registerError) =>
                            {
                                SendRequestPermissionResult(callback, permissionStatus, accessError);
                            });
                        }
                        else
                        {
                            SendRequestPermissionResult(callback, permissionStatus, accessError);
                        }
                    });
                });
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        private static void SendRequestPermissionResult(EventCallback<NotificationServicesRequestPermissionResult> callback, NotificationPermissionStatus permissionStatus, Error error)
        {
            // send result to caller object
            var     result          = new NotificationServicesRequestPermissionResult()
            {
                PermissionStatus    = permissionStatus,
            };
            CallbackDispatcher.InvokeOnMainThread(callback, result, error);
        }

        private static void GetSettingsInternal(bool sendUpdateEvent, Callback<NotificationServicesGetSettingsResult> callback = null)
        {
            try
            {
                // make request
                s_nativeInterface.GetSettings((settings) =>
                {
                    // cache result
                    var     newSettings     = new NotificationSettings(
                        permissionStatus: settings.PermissionStatus, 
                        alertSetting: settings.AlertSetting, 
                        badgeSetting: settings.BadgeSetting,
                        carPlaySetting: settings.CarPlaySetting, 
                        lockScreenSetting: settings.LockScreenSetting, 
                        notificationCenterSetting: settings.NotificationCenterSetting, 
                        soundSetting: settings.SoundSetting, 
                        criticalAlertSetting: settings.CriticalAlertSetting, 
                        announcementSetting: settings.AnnouncementSetting, 
                        alertStyle: settings.AlertStyle, 
                        previewStyle: settings.PreviewStyle, 
                        pushNotificationEnabled: (s_deviceToken != null), 
                        deviceToken: s_deviceToken);
                    CachedSettings          = newSettings;

                    // notify listeners
                    var     result          = new NotificationServicesGetSettingsResult()
                    {
                        Settings            = newSettings,
                    };
                    CallbackDispatcher.InvokeOnMainThread(callback, result);
                    if (sendUpdateEvent)
                    {
                        CallbackDispatcher.InvokeOnMainThread(OnSettingsUpdate, newSettings);
                    }
                });
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
        }

        private static void CopyPushNotificationPropertiesToCachedSettings(bool sendUpdateEvent = true)
        {
            if (CachedSettings == null)
            {
                return;
            }

            // update local copy
            var     newSettingsCopy     = new NotificationSettings(
                permissionStatus: CachedSettings.PermissionStatus, 
                alertSetting: CachedSettings.AlertSetting, 
                badgeSetting: CachedSettings.BadgeSetting,
                carPlaySetting: CachedSettings.CarPlaySetting, 
                lockScreenSetting: CachedSettings.LockScreenSetting, 
                notificationCenterSetting: CachedSettings.NotificationCenterSetting, 
                soundSetting: CachedSettings.SoundSetting, 
                criticalAlertSetting: CachedSettings.CriticalAlertSetting, 
                announcementSetting: CachedSettings.AnnouncementSetting, 
                alertStyle: CachedSettings.AlertStyle, 
                previewStyle: CachedSettings.PreviewStyle, 
                pushNotificationEnabled: (s_deviceToken != null), 
                deviceToken: s_deviceToken);
            CachedSettings  = newSettingsCopy;

            // send event
            if (sendUpdateEvent)
            {
                CallbackDispatcher.InvokeOnMainThread(OnSettingsUpdate, newSettingsCopy);
            }
        }

        #endregion

        #region Event callback methods

        private static void HandleNotificationReceivedInternalCallback(INotification notification)
        {
            // send result to caller object
            var     result      = new NotificationServicesNotificationReceivedResult()
            {
                Notification    = notification,
            };
            if (OnNotificationReceived != null)
            {
                CallbackDispatcher.InvokeOnMainThread(OnNotificationReceived, result);
            }
            else
            {
                SurrogateCoroutine.WaitUntilAndInvoke(new WaitUntil(() => (OnNotificationReceived != null)), () =>
                {
                    CallbackDispatcher.InvokeOnMainThread(OnNotificationReceived, result);
                });
            }
        }

        #endregion
    }
}