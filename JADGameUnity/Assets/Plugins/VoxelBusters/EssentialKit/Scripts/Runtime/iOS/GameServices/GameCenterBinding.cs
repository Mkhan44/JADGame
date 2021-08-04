#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal static class GameCenterBinding
    {
        [DllImport("__Internal")]
        public static extern void NPGameServicesSetViewClosedCallback(GameServicesViewClosedNativeCallback viewClosedCallback);

        [DllImport("__Internal")]
        public static extern void NPGameServicesLoadServerCredentials(IntPtr tagPtr);

        [DllImport("__Internal")]
        public static extern void NPGameServicesLoadServerCredentialsCompleteCallback(GameServicesLoadServerCredentialsNativeCallback serverCredentialsCallback);
    }
}
#endif