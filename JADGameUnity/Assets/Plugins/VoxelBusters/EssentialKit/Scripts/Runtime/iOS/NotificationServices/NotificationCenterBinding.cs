#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{
    internal static class NotificationCenterBinding
    {
        [DllImport("__Internal")]
        public static extern void NPNotificationCenterRegisterCallbacks(RequestAuthorizationNativeCallback requestAuthorizationCallback,
                                                                GetSettingsNativeCallback getSettingsCallback,
                                                                ScheduleNotificationNativeCallback scheduleLocalNotificationCallback,
                                                                GetScheduledNotificationsNativeCallback getScheduledNotificationsCallback,
                                                                GetDeliveredNotificationsNativeCallback getDeliveredNotificationsCallback,
                                                                RegisterForRemoteNotificationsNativeCallback registerForRemoteNotificationCallback,
                                                                NotificationReceivedNativeCallback notificationReceivedCallback);

        [DllImport("__Internal")]
        public static extern void NPNotificationCenterInit(UNNotificationPresentationOptions presentationOptions);

        [DllImport("__Internal")]
        public static extern void NPNotificationCenterRequestAuthorization(UNAuthorizationOptions options, IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern void NPNotificationCenterGetSettings(IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern void NPNotificationCenterScheduleLocalNotification(IntPtr notificationRequestPtr, IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern void NPNotificationCenterGetScheduledNotifications(IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern void NPNotificationCenterRemovePendingNotification(string notificationId);

        [DllImport("__Internal")]
        public static extern void NPNotificationCenterRemoveAllPendingNotifications();

        [DllImport("__Internal")]
        public static extern void NPNotificationCenterRemoveAllDeliveredNotifications();

        [DllImport("__Internal")]
        public static extern void NPNotificationCenterGetDeliveredNotifications(IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern void NPNotificationCenterRegisterForRemoteNotifications(IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern bool NPNotificationCenterIsRegisteredForRemoteNotifications();

        [DllImport("__Internal")]
        public static extern void NPNotificationCenterUnregisterForRemoteNotifications();

        [DllImport("__Internal")]
        public static extern void NPNotificationCenterSetApplicationIconBadgeNumber(int count);
    }
}
#endif