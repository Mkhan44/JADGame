#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.MediaServicesCore.iOS
{
    internal enum AVAuthorizationStatus : long
    {
        AVAuthorizationStatusNotDetermined,

        AVAuthorizationStatusRestricted,

        AVAuthorizationStatusDenied,

        AVAuthorizationStatusAuthorized,
    }
}
#endif