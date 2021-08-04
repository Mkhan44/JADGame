#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.CloudServicesCore.iOS
{
    internal delegate void UserChangeNativeCallback(ref CKAccountData accountData, string error);

    internal delegate void SavedDataChangeNativeCallback(int changeReason, ref NativeArray changedKeys);
}
#endif