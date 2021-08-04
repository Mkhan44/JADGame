#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal static class AchievementBinding
    {
        [DllImport("__Internal")]
        public static extern void NPAchievementRegisterCallbacks(GameServicesLoadArrayNativeCallback loadAchievementsCallback, GameServicesReportNativeCallback reportAchievementCallback);

        [DllImport("__Internal")]
        public static extern void NPAchievementSetCanShowBannerOnCompletion(bool showsBannerOnCompletion);

        [DllImport("__Internal")]
        public static extern void NPAchievementLoadAchievements(IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern IntPtr NPAchievementCreate(string id);

        [DllImport("__Internal")]
        public static extern string NPAchievementGetId(IntPtr achievementPtr);

        [DllImport("__Internal")]
        public static extern double NPAchievementGetPercentageCompleted(IntPtr achievementPtr);

        [DllImport("__Internal")]
        public static extern void NPAchievementSetPercentageCompleted(IntPtr achievementPtr, double percentComplete);

        [DllImport("__Internal")]
        public static extern bool NPAchievementGetIsCompleted(IntPtr achievementPtr);

        [DllImport("__Internal")]
        public static extern string NPAchievementGetLastReportedDate(IntPtr achievementPtr);

        [DllImport("__Internal")]
        public static extern void NPAchievementReportProgress(IntPtr achievementPtr, IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern void NPAchievementShowView(IntPtr tagPtr);
    }
}
#endif