#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{
    internal delegate void RequestAuthorizationNativeCallback(UNAuthorizationStatus status, string error, IntPtr tagPtr);

    internal delegate void GetSettingsNativeCallback(ref UNNotificationSettingsData settings, IntPtr tagPtr);

    internal delegate void ScheduleNotificationNativeCallback(string error, IntPtr tagPtr);

    internal delegate void GetScheduledNotificationsNativeCallback(ref NativeArray arrayPtr, string error, IntPtr tagPtr);

    internal delegate void GetDeliveredNotificationsNativeCallback(ref NativeArray arrayPtr, string error, IntPtr tagPtr);

    internal delegate void RegisterForRemoteNotificationsNativeCallback(string deviceToken, string error, IntPtr tagPtr);

    internal delegate void NotificationReceivedNativeCallback(IntPtr nativePtr, bool isLaunchNotification);
}
#endif