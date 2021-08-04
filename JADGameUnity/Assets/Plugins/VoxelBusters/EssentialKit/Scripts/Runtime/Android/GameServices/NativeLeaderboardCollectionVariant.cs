#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    public enum NativeLeaderboardCollectionVariant
    {
        Public = 0,
        Friends = 1
    }
    public class NativeLeaderboardCollectionVariantHelper
    {
        internal const string kClassName = "com.voxelbusters.android.essentialkit.features.gameservices.LeaderboardCollectionVariant";

        public static AndroidJavaObject CreateWithValue(NativeLeaderboardCollectionVariant value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeLeaderboardCollectionVariantHelper : NativeLeaderboardCollectionVariantHelper][Method(CreateWithValue) : NativeLeaderboardCollectionVariant]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeLeaderboardCollectionVariant ReadFromValue(AndroidJavaObject value)
        {
            return (NativeLeaderboardCollectionVariant)value.Call<int>("ordinal");
        }
    }
}
#endif