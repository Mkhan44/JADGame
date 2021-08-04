#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.MediaServicesCore.Android
{
    public enum NativeGalleryAccessStatus
    {
        Denied = 0,
        Authorized = 1
    }
    public class NativeGalleryAccessStatusHelper
    {
        internal const string kClassName = "com.voxelbusters.android.essentialkit.features.mediaservices.GalleryAccessStatus";

        public static AndroidJavaObject CreateWithValue(NativeGalleryAccessStatus value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeGalleryAccessStatusHelper : NativeGalleryAccessStatusHelper][Method(CreateWithValue) : NativeGalleryAccessStatus]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeGalleryAccessStatus ReadFromValue(AndroidJavaObject value)
        {
            return (NativeGalleryAccessStatus)value.Call<int>("ordinal");
        }
    }
}
#endif