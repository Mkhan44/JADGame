#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal static class LeaderboardBinding
    {
        [DllImport("__Internal")]
        public static extern void NPLeaderboardRegisterCallbacks(GameServicesLoadArrayNativeCallback loadLeaderboardsCallback, GameServicesLoadArrayNativeCallback loadScoresCallback, GameServicesLoadImageNativeCallback loadImageCallback);

        [DllImport("__Internal")]
        public static extern void NPLeaderboardLoadLeaderboards(IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern IntPtr NPLeaderboardCreate(string id);

        [DllImport("__Internal")]
        public static extern string NPLeaderboardGetId(IntPtr leaderboardPtr);

        [DllImport("__Internal")]
        public static extern string NPLeaderboardGetTitle(IntPtr leaderboardPtr);

        [DllImport("__Internal")]
        public static extern GKLeaderboardPlayerScope NPLeaderboardGetPlayerScope(IntPtr leaderboardPtr);

        [DllImport("__Internal")]
        public static extern void NPLeaderboardSetPlayerScope(IntPtr leaderboardPtr, GKLeaderboardPlayerScope playerScope);

        [DllImport("__Internal")]
        public static extern GKLeaderboardTimeScope NPLeaderboardGetTimeScope(IntPtr leaderboardPtr);

        [DllImport("__Internal")]
        public static extern void NPLeaderboardSetTimeScope(IntPtr leaderboardPtr, GKLeaderboardTimeScope timeScope);

        [DllImport("__Internal")]
        public static extern IntPtr NPLeaderboardGetLocalPlayerScore(IntPtr leaderboardPtr);

        [DllImport("__Internal")]
        public static extern void NPLeaderboardLoadScores(IntPtr leaderboardPtr, long startIndex, int count, IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern void NPLeaderboardLoadImage(IntPtr leaderboardPtr, IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern void NPLeaderboardShowView(string leaderboardID, GKLeaderboardTimeScope timeScope, IntPtr tagPtr);
    }
}
#endif