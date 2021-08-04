#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal static class PlayerBinding
    {
        [DllImport("__Internal")]
        public static extern void NPPlayerRegisterCallbacks(GameServicesLoadArrayNativeCallback loadPlayersCallback, GameServicesLoadImageNativeCallback loadPlayerImageCallback);

        [DllImport("__Internal")]
        public static extern void NPPlayerLoadPlayers(string[] playerIds, int count, IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern string NPPlayerGetId(IntPtr playerPtr);

        [DllImport("__Internal")]
        public static extern string NPPlayerGetAlias(IntPtr playerPtr);

        [DllImport("__Internal")]
        public static extern string NPPlayerGetDisplayName(IntPtr playerPtr);

        [DllImport("__Internal")]
        public static extern void NPPlayerLoadImage(IntPtr playerPtr, IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern void NPLocalPlayerRegisterCallbacks(GameServicesAuthStateChangeNativeCallback authChangeCallback);

        [DllImport("__Internal")]
        public static extern IntPtr NPLocalPlayerGetLocalPlayer();

        [DllImport("__Internal")]
        public static extern void NPLocalPlayerAuthenticate();

        [DllImport("__Internal")]
        public static extern bool NPLocalPlayerIsAuthenticated();

        [DllImport("__Internal")]
        public static extern bool NPLocalPlayerIsUnderage();
    }
}
#endif