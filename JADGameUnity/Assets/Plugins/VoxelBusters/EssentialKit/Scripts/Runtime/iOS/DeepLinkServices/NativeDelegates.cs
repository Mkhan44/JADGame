#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.DeepLinkServicesCore.iOS
{
    internal delegate bool HandleCustomSchemeUrlNativeCallback(string url);

    internal delegate bool HandleUniversalLinkNativeCallback(string url);
}
#endif