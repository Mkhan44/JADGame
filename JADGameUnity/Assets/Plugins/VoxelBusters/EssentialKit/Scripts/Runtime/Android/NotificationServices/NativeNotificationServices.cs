#if UNITY_ANDROID
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    public class NativeNotificationServices : NativeAndroidJavaObjectWrapper
    {
        #region Static properties

         private static AndroidJavaClass m_nativeClass;

        #endregion
        #region Private properties
        private NativeActivity Activity
        {
            get;
            set;
        }
        #endregion

        #region Constructor

        public NativeNotificationServices(NativeContext context) : base(Native.kClassName, (object)context.NativeObject)
        {
            Activity    = new NativeActivity(context);
        }

        #endregion
        #region Static methods
        private static AndroidJavaClass GetClass()
        {
            if (m_nativeClass == null)
            {
                m_nativeClass = new AndroidJavaClass(Native.kClassName);
            }
            return m_nativeClass;
        }
        public static void ProcessLaunchNotification(NativeNotification notification)
        {
            GetClass().CallStatic(Native.Method.kProcessLaunchNotification, notification.NativeObject);
        }

        #endregion
        #region Public methods

        public string GetFeatureName()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : GetFeatureName]");
#endif
            return Call<string>(Native.Method.kGetFeatureName);
        }
        public NativeNotificationType GetNotificationType()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : GetNotificationType]");
#endif
            AndroidJavaObject nativeObj = Call<AndroidJavaObject>(Native.Method.kGetNotificationType);
            NativeNotificationType data  = NativeNotificationTypeHelper.ReadFromValue(nativeObj);
            return data;
        }
        public void SetNotificationType(NativeNotificationType type)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : SetNotificationType]");
#endif
            Call(Native.Method.kSetNotificationType, NativeNotificationTypeHelper.CreateWithValue(type));
        }
        public void SetNotificationListener(NativeNotificationReceivedListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : SetNotificationListener]");
#endif
            Call(Native.Method.kSetNotificationListener, listener);
        }
        public void RequestSystemNotificationSettings(NativeRequestSystemNotificationSettingsListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : RequestSystemNotificationSettings]");
#endif
            Call(Native.Method.kRequestSystemNotificationSettings, listener);
        }
        public void ScheduleNotification(NativeNotification notification, NativeScheduleNotificationListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : ScheduleNotification]");
#endif
            Call(Native.Method.kScheduleNotification, notification.NativeObject, listener);
        }
        public void RequestScheduledNotifications(NativeNotificationsRequestListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : RequestScheduledNotifications]");
#endif
            Call(Native.Method.kRequestScheduledNotifications, listener);
        }
        public void CancelScheduledNotification(string id)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : CancelScheduledNotification]");
#endif
            Call(Native.Method.kCancelScheduledNotification, id);
        }
        public void CancelAllScheduledNotifications()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : CancelAllScheduledNotifications]");
#endif
            Call(Native.Method.kCancelAllScheduledNotifications);
        }
        public void RequestActiveNotifications(NativeNotificationsRequestListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : RequestActiveNotifications]");
#endif
            Call(Native.Method.kRequestActiveNotifications, listener);
        }
        public void ClearAllActiveNotifications()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : ClearAllActiveNotifications]");
#endif
            Call(Native.Method.kClearAllActiveNotifications);
        }
        public void RegisterRemoteNotifications(NativeRegisterRemoteNotificationsListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : RegisterRemoteNotifications]");
#endif
            Call(Native.Method.kRegisterRemoteNotifications, listener);
        }
        public void UnregisterRemoteNotifications(NativeUnregisterRemoteNotificationServiceListener listener)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : UnregisterRemoteNotifications]");
#endif
            Call(Native.Method.kUnregisterRemoteNotifications, listener);
        }
        public bool AreNotificationsAllowedByUser()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : AreNotificationsAllowedByUser]");
#endif
            return Call<bool>(Native.Method.kAreNotificationsAllowedByUser);
        }
        public bool AreSoundsEnabledByUser()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : AreSoundsEnabledByUser]");
#endif
            return Call<bool>(Native.Method.kAreSoundsEnabledByUser);
        }
        public bool AreRemoteNotificationsAvailable()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : AreRemoteNotificationsAvailable]");
#endif
            return Call<bool>(Native.Method.kAreRemoteNotificationsAvailable);
        }
        public bool AreRemoteNotificationsRegistered()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : AreRemoteNotificationsRegistered]");
#endif
            return Call<bool>(Native.Method.kAreRemoteNotificationsRegistered);
        }
        public void SetApplicationIconBadgeNumber(int count)
        {
            Activity.RunOnUiThread(() => {
#if NATIVE_PLUGINS_DEBUG_ENABLED
                DebugLogger.Log("[Class : NativeNotificationServices][Method(RunOnUiThread) : SetApplicationIconBadgeNumber]");
#endif
                Call(Native.Method.kSetApplicationIconBadgeNumber, count);
            });
        }
        public void RefreshActiveNotificationsStore()
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[Class : NativeNotificationServices][Method : RefreshActiveNotificationsStore]");
#endif
            Call(Native.Method.kRefreshActiveNotificationsStore);
        }

        #endregion

        internal class Native
        {
            internal const string kClassName = "com.voxelbusters.android.essentialkit.features.notificationservices.NotificationServices";

            internal class Method
            {
                internal const string kUnregisterRemoteNotifications = "UnregisterRemoteNotifications";
                internal const string kScheduleNotification = "scheduleNotification";
                internal const string kAreSoundsEnabledByUser = "areSoundsEnabledByUser";
                internal const string kRequestSystemNotificationSettings = "requestSystemNotificationSettings";
                internal const string kGetNotificationType = "getNotificationType";
                internal const string kSetNotificationType = "setNotificationType";
                internal const string kRefreshActiveNotificationsStore = "refreshActiveNotificationsStore";
                internal const string kCancelAllScheduledNotifications = "cancelAllScheduledNotifications";
                internal const string kAreRemoteNotificationsAvailable = "areRemoteNotificationsAvailable";
                internal const string kProcessLaunchNotification = "processLaunchNotification";
                internal const string kSetNotificationListener = "setNotificationListener";
                internal const string kGetFeatureName = "getFeatureName";
                internal const string kRequestActiveNotifications = "requestActiveNotifications";
                internal const string kClearAllActiveNotifications = "clearAllActiveNotifications";
                internal const string kRegisterRemoteNotifications = "registerRemoteNotifications";
                internal const string kAreNotificationsAllowedByUser = "areNotificationsAllowedByUser";
                internal const string kCancelScheduledNotification = "cancelScheduledNotification";
                internal const string kRequestScheduledNotifications = "requestScheduledNotifications";
                internal const string kSetApplicationIconBadgeNumber = "setApplicationIconBadgeNumber";
                internal const string kAreRemoteNotificationsRegistered = "areRemoteNotificationsRegistered";
            }

        }
    }
}
#endif