#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal static class ScoreBinding
    {
        [DllImport("__Internal")]
        public static extern void NPScoreRegisterCallbacks(GameServicesReportNativeCallback reportScoreCallback);

        [DllImport("__Internal")]
        public static extern IntPtr NPScoreCreate(string leaderboardId);

        [DllImport("__Internal")]
        public static extern string NPScoreGetLeaderboardId(IntPtr scorePtr);

        [DllImport("__Internal")]
        public static extern long NPScoreGetRank(IntPtr scorePtr);

        [DllImport("__Internal")]
        public static extern long NPScoreGetValue(IntPtr scorePtr);

        [DllImport("__Internal")]
        public static extern void NPScoreSetValue(IntPtr scorePtr, long value);

        [DllImport("__Internal")]
        public static extern string NPScoreGetLastReportedDate(IntPtr scorePtr);

        [DllImport("__Internal")]
        public static extern IntPtr NPScoreGetPlayer(IntPtr scorePtr);

        [DllImport("__Internal")]
        public static extern void NPScoreReportScore(IntPtr scorePtr, IntPtr tagPtr);
    }
}
#endif