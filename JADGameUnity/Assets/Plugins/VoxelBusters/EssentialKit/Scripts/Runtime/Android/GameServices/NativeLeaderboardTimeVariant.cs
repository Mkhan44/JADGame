#if UNITY_ANDROID
using UnityEngine;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.GameServicesCore.Android
{
    public enum NativeLeaderboardTimeVariant
    {
        Daily = 0,
        Weekly = 1,
        AllTime = 2
    }
    public class NativeLeaderboardTimeVariantHelper
    {
        internal const string kClassName = "com.voxelbusters.android.essentialkit.features.gameservices.LeaderboardTimeVariant";

        public static AndroidJavaObject CreateWithValue(NativeLeaderboardTimeVariant value)
        {
#if NATIVE_PLUGINS_DEBUG_ENABLED
            DebugLogger.Log("[NativeLeaderboardTimeVariantHelper : NativeLeaderboardTimeVariantHelper][Method(CreateWithValue) : NativeLeaderboardTimeVariant]");
#endif
            AndroidJavaClass javaClass = new AndroidJavaClass(kClassName);
            AndroidJavaObject[] values = javaClass.CallStatic<AndroidJavaObject[]>("values");
            return values[(int)value];
        }

        public static NativeLeaderboardTimeVariant ReadFromValue(AndroidJavaObject value)
        {
            return (NativeLeaderboardTimeVariant)value.Call<int>("ordinal");
        }
    }
}
#endif