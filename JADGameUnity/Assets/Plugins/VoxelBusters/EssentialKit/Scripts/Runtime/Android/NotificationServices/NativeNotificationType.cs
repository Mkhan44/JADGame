#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    public enum NativeNotificationType
    {
        Unknown = 0,
        None = 1,
        Badge = 2,
        Sound = 3,
        BadgeAndSound = 4,
        Alert = 5,
        AlertAndBadge = 6,
        AlertAndSound = 7,
        All = 8
    }
    public class NativeNotificationTypeHelper
    {
        internal const string kClassName = "com.voxelbusters.android.essentialkit.features.notificationservices.datatypes.NotificationType";

        public static AndroidJavaObject CreateWithValue(NativeNotificationType value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeNotificationTypeHelper : NativeNotificationTypeHelper][Method(CreateWithValue) : NativeNotificationType]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeNotificationType ReadFromValue(AndroidJavaObject value)
        {
            return (NativeNotificationType)value.Call<int>("ordinal");
        }
    }
}
#endif