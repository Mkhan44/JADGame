#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    public enum NativeNotificationImportance
    {
        Min = 0,
        Low = 1,
        Default = 2,
        High = 3,
        Max = 4
    }
    public class NativeNotificationImportanceHelper
    {
        internal const string kClassName = "com.voxelbusters.android.essentialkit.features.notificationservices.datatypes.NotificationImportance";

        public static AndroidJavaObject CreateWithValue(NativeNotificationImportance value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeNotificationImportanceHelper : NativeNotificationImportanceHelper][Method(CreateWithValue) : NativeNotificationImportance]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeNotificationImportance ReadFromValue(AndroidJavaObject value)
        {
            return (NativeNotificationImportance)value.Call<int>("ordinal");
        }
    }
}
#endif