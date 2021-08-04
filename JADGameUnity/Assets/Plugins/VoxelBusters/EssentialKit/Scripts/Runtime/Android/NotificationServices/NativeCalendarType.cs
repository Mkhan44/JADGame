#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    public enum NativeCalendarType
    {
        Default = 0
    }
    public class NativeCalendarTypeHelper
    {
        internal const string kClassName = "com.voxelbusters.android.essentialkit.features.notificationservices.datatypes.CalendarType";

        public static AndroidJavaObject CreateWithValue(NativeCalendarType value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeCalendarTypeHelper : NativeCalendarTypeHelper][Method(CreateWithValue) : NativeCalendarType]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeCalendarType ReadFromValue(AndroidJavaObject value)
        {
            return (NativeCalendarType)value.Call<int>("ordinal");
        }
    }
}
#endif