#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    public enum NativeNotificationAccessState
    {
        Denied = 0,
        Authorized = 1,
        Unknown = 2
    }
    public class NativeNotificationAccessStateHelper
    {
        internal const string kClassName = "com.voxelbusters.android.essentialkit.features.notificationservices.datatypes.NotificationAccessState";

        public static AndroidJavaObject CreateWithValue(NativeNotificationAccessState value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeNotificationAccessStateHelper : NativeNotificationAccessStateHelper][Method(CreateWithValue) : NativeNotificationAccessState]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeNotificationAccessState ReadFromValue(AndroidJavaObject value)
        {
            return (NativeNotificationAccessState)value.Call<int>("ordinal");
        }
    }
}
#endif