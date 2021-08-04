#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.MediaServicesCore.Android
{
    public enum NativeCameraAccessStatus
    {
        Denied = 0,
        Authorized = 1
    }
    public class NativeCameraAccessStatusHelper
    {
        internal const string kClassName = "com.voxelbusters.android.essentialkit.features.mediaservices.CameraAccessStatus";

        public static AndroidJavaObject CreateWithValue(NativeCameraAccessStatus value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeCameraAccessStatusHelper : NativeCameraAccessStatusHelper][Method(CreateWithValue) : NativeCameraAccessStatus]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeCameraAccessStatus ReadFromValue(AndroidJavaObject value)
        {
            return (NativeCameraAccessStatus)value.Call<int>("ordinal");
        }
    }
}
#endif