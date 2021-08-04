#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal static class AchievementDescriptionBinding
    {
        [DllImport("__Internal")]
        public static extern void NPAchievementDescriptionRegisterCallbacks(GameServicesLoadArrayNativeCallback loadDescriptionsCallback, GameServicesLoadImageNativeCallback loadImageCallback);

        [DllImport("__Internal")]
        public static extern void NPAchievementDescriptionLoadDescriptions(IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern string NPAchievementDescriptionGetId(IntPtr descriptionPtr);
    
        [DllImport("__Internal")]
        public static extern string NPAchievementDescriptionGetTitle(IntPtr descriptionPtr);

        [DllImport("__Internal")]
        public static extern string NPAchievementDescriptionGetAchievedDescription(IntPtr descriptionPtr);

        [DllImport("__Internal")]
        public static extern string NPAchievementDescriptionGetUnachievedDescription(IntPtr descriptionPtr);

        [DllImport("__Internal")]
        public static extern long NPAchievementDescriptionGetMaximumPoints(IntPtr descriptionPtr);

        [DllImport("__Internal")]
        public static extern bool NPAchievementDescriptionGetHidden(IntPtr descriptionPtr);

        [DllImport("__Internal")]
        public static extern bool NPAchievementDescriptionGetReplayable(IntPtr descriptionPtr);

        [DllImport("__Internal")]
        public static extern void NPAchievementDescriptionLoadIncompleteAchievementImage(IntPtr descriptionPtr, IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern void NPAchievementDescriptionLoadImage(IntPtr descriptionPtr, IntPtr tagPtr);
    }
}
#endif