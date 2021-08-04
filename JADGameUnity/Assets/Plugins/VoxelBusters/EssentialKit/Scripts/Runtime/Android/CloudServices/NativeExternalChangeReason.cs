#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.CloudServicesCore.Android
{
    public enum NativeExternalChangeReason
    {
        InitialSync = 0,
        ServerSync = 1,
        UserChange = 2
    }
    public class NativeExternalChangeReasonHelper
    {
        internal const string kClassName = "com.voxelbusters.android.essentialkit.features.cloudservices.ExternalChangeReason";

        public static AndroidJavaObject CreateWithValue(NativeExternalChangeReason value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeExternalChangeReasonHelper : NativeExternalChangeReasonHelper][Method(CreateWithValue) : NativeExternalChangeReason]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeExternalChangeReason ReadFromValue(AndroidJavaObject value)
        {
            return (NativeExternalChangeReason)value.Call<int>("ordinal");
        }
    }
}
#endif