#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using AOT;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{
    public sealed class NotificationCenterInterface : NativeNotificationCenterInterfaceBase, INativeNotificationCenterInterface
    {
        #region Static fields

        private     static      NotificationCenterInterface         s_sharedInstance        = null;

        #endregion

        #region Constructors

        public NotificationCenterInterface() 
            : base(isAvailable: true)
        {
            if (s_sharedInstance == null)
            {
                // save reference
                s_sharedInstance        = this;

                // initialise native component
                var     unitySettings   = NotificationServices.UnitySettings;
                NotificationCenterBinding.NPNotificationCenterRegisterCallbacks(HandleRequestAuthorizationCallback, HandleGetSettingsCallback, HandleScheduleNotificationCallback, HandleGetScheduledNotificationsCallback, HandleGetDeliveredNotificationsCallback, HandleRegisterForRemoteNotificationsCallback, HandleNotificationReceivedCallback);
                NotificationCenterBinding.NPNotificationCenterInit(NotificationServicesUtility.ConvertToUNNotificationPresentationOptions(unitySettings.PresentationOptions));
            }
        }

        #endregion

        #region Base class methods

        public override void RequestPermission(NotificationPermissionOptions options, RequestPermissionInternalCallback callback)
        { 
            // get native representation
            var     authorizationOptions = NotificationServicesUtility.ConvertToUNAuthorizationOptions(options);

            // make request
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            NotificationCenterBinding.NPNotificationCenterRequestAuthorization(authorizationOptions, tagPtr);
        }

        public override void GetSettings(GetSettingsInternalCallback callback)
        { 
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            NotificationCenterBinding.NPNotificationCenterGetSettings(tagPtr);
        }

        public override IMutableNotification CreateMutableNotification(string notificationId)
        {
            return new MutableNotification(notificationId);
        }

        public override void ScheduleNotification(INotification notification, ScheduleNotificationInternalCallback callback)
        { 
            var     requestPtr  = ((NotificationBase)notification).AddrOfNativeObject();
            if (requestPtr != IntPtr.Zero)
            {
                callback(new Error(description: "The operation could not be completed as request object was already scheduled"));
                return;
            }

            // create request object
            var     mutableNotification = (MutableNotification)notification;
            var     nativeContent       = mutableNotification.GetNativeContentInternal();
            var     nativeTrigger       = mutableNotification.GetNativeTriggerInternal();

            var     contentPtr  = (nativeContent == null) ? IntPtr.Zero : nativeContent.Pointer;
            var     triggerPtr  = (nativeTrigger == null) ? IntPtr.Zero : nativeTrigger.Pointer;
            requestPtr          = NotificationBinding.NPNotificationRequestCreate(mutableNotification.Id, contentPtr, triggerPtr);
            mutableNotification.SetRequestPtr(requestPtr, retain: false);

            // schedule notification
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            NotificationCenterBinding.NPNotificationCenterScheduleLocalNotification(requestPtr, tagPtr);
        }

        public override void GetScheduledNotifications(GetNotificationsInternalCallback callback)
        { 
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            NotificationCenterBinding.NPNotificationCenterGetScheduledNotifications(tagPtr);
        }

        public override void CancelScheduledNotification(string notificationId)
        { 
            NotificationCenterBinding.NPNotificationCenterRemovePendingNotification(notificationId);
        }

        public override void CancelAllScheduledNotifications()
        { 
            NotificationCenterBinding.NPNotificationCenterRemoveAllPendingNotifications();
        }

        public override void GetDeliveredNotifications(GetNotificationsInternalCallback callback)
        { 
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            NotificationCenterBinding.NPNotificationCenterGetDeliveredNotifications(tagPtr);
        }

        public override void RemoveAllDeliveredNotifications()
        { 
            NotificationCenterBinding.NPNotificationCenterRemoveAllDeliveredNotifications();
        }
        
        public override void RegisterForPushNotifications(RegisterForPushNotificationsInternalCallback callback)
        { 
            var     tagPtr      = MarshalUtility.GetIntPtr(callback);
            NotificationCenterBinding.NPNotificationCenterRegisterForRemoteNotifications(tagPtr);
        }

        public override void UnregisterForPushNotifications()
        {
             NotificationCenterBinding.NPNotificationCenterUnregisterForRemoteNotifications();
        }

        public override bool IsRegisteredForPushNotifications()
        { 
            return NotificationCenterBinding.NPNotificationCenterIsRegisteredForRemoteNotifications();
        }

        public override void SetApplicationIconBadgeNumber(int count)
        {
            NotificationCenterBinding.NPNotificationCenterSetApplicationIconBadgeNumber(count);
        }

        #endregion

        #region Native callback methods

        [MonoPInvokeCallback(typeof(RequestAuthorizationNativeCallback))]
        private static void HandleRequestAuthorizationCallback(UNAuthorizationStatus status, string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     permissionStatus    = NotificationServicesUtility.ConvertToNotificationPermissionStatus(status);
                var     errorObj            = Error.CreateNullableError(description: error);
                ((RequestPermissionInternalCallback)tagHandle.Target).Invoke(permissionStatus, errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        [MonoPInvokeCallback(typeof(GetSettingsNativeCallback))]
        private static void HandleGetSettingsCallback(ref UNNotificationSettingsData settings, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     unitySettings   = new NotificationSettingsInternal(
                    permissionStatus: NotificationServicesUtility.ConvertToNotificationPermissionStatus(settings.AuthorizationStatus), 
                    alertSetting: NotificationServicesUtility.ConvertToNotificationSettingStatus(settings.AlertSetting), 
                    badgeSetting: NotificationServicesUtility.ConvertToNotificationSettingStatus(settings.BadgeSetting),
                    carPlaySetting: NotificationServicesUtility.ConvertToNotificationSettingStatus(settings.CarPlaySetting), 
                    lockScreenSetting: NotificationServicesUtility.ConvertToNotificationSettingStatus(settings.LockScreenSetting), 
                    notificationCenterSetting: NotificationServicesUtility.ConvertToNotificationSettingStatus(settings.NotificationCenterSetting), 
                    soundSetting: NotificationServicesUtility.ConvertToNotificationSettingStatus(settings.SoundSetting), 
                    criticalAlertSetting: NotificationServicesUtility.ConvertToNotificationSettingStatus(settings.CriticalAlertSetting), 
                    announcementSetting: NotificationServicesUtility.ConvertToNotificationSettingStatus(settings.AnnouncementSetting), 
                    alertStyle: NotificationServicesUtility.ConvertToNotificationAlertStyle(settings.AlertStyle), 
                    previewStyle: NotificationServicesUtility.ConvertToNotificationPreviewStyle(settings.ShowPreviewsSetting));
                ((GetSettingsInternalCallback)tagHandle.Target).Invoke(unitySettings);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        [MonoPInvokeCallback(typeof(ScheduleNotificationNativeCallback))]
        private static void HandleScheduleNotificationCallback(string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     errorObj    = Error.CreateNullableError(description: error);
                ((ScheduleNotificationInternalCallback)tagHandle.Target).Invoke(errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        [MonoPInvokeCallback(typeof(GetScheduledNotificationsNativeCallback))]
        private static void HandleGetScheduledNotificationsCallback(ref NativeArray arrayPtr, string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     nativeArray     = MarshalUtility.CreateManagedArray(arrayPtr.Pointer, arrayPtr.Length);
                var     notifications   = (nativeArray == null) 
                    ? null 
                    : Array.ConvertAll(nativeArray, (nativePtr) => new Notification(nativePtr));
                var     errorObj        = Error.CreateNullableError(description: error);
                ((GetNotificationsInternalCallback)tagHandle.Target).Invoke(notifications, errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        [MonoPInvokeCallback(typeof(GetDeliveredNotificationsNativeCallback))]
        private static void HandleGetDeliveredNotificationsCallback(ref NativeArray arrayPtr, string error, IntPtr tagPtr)
        {
            GCHandle    tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                // send result
                var     nativeArray     = MarshalUtility.CreateManagedArray(arrayPtr.Pointer, arrayPtr.Length);
                var     notifications   = (nativeArray == null) 
                    ? null 
                    : Array.ConvertAll(nativeArray, (nativePtr) => new Notification(nativePtr));
                var     errorObj        = Error.CreateNullableError(description: error);
                ((GetNotificationsInternalCallback)tagHandle.Target).Invoke(notifications, errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }


        [MonoPInvokeCallback(typeof(RegisterForRemoteNotificationsNativeCallback))]
        private static void HandleRegisterForRemoteNotificationsCallback(string deviceToken, string error, IntPtr tagPtr)
        {
            var     tagHandle   = GCHandle.FromIntPtr(tagPtr);

            try
            {
                var     errorObj    = Error.CreateNullableError(description: error);
                ((RegisterForPushNotificationsInternalCallback)tagHandle.Target).Invoke(deviceToken, errorObj);
            }
            catch (Exception exception)
            {
                DebugLogger.LogException(exception);
            }
            finally
            {
                // release handle
                tagHandle.Free();
            }
        }

        [MonoPInvokeCallback(typeof(NotificationReceivedNativeCallback))]
        private static void HandleNotificationReceivedCallback(IntPtr nativePtr, bool isLaunchNotification)
        {
            // send result
            var     notification    = new Notification(nativePtr);
            s_sharedInstance.SendNotificationReceivedEvent(notification);
        }

        #endregion
    }
}
#endif