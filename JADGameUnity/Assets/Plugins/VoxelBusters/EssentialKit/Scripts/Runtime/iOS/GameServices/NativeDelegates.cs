#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal delegate void GameServicesLoadArrayNativeCallback(ref NativeArray nativeArray, string error, IntPtr tagPtr);

    internal delegate void GameServicesReportNativeCallback(string error, IntPtr tagPtr);

    internal delegate void GameServicesAuthStateChangeNativeCallback(GKLocalPlayerAuthState state, string error);

    internal delegate void GameServicesLoadImageNativeCallback(IntPtr dataPtr, int dataLength, string error, IntPtr tagPtr);

    internal delegate void GameServicesViewClosedNativeCallback(string error, IntPtr tagPtr);

    internal delegate void GameServicesLoadServerCredentialsNativeCallback(string publicKeyUrl, IntPtr signaturePtr, int signatureDataLength, IntPtr saltPtr, int saltDataLength, long timestamp, string error, IntPtr tagPtr);
}
#endif