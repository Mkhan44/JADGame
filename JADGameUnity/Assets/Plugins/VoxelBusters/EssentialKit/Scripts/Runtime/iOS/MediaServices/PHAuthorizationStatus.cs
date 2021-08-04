#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.MediaServicesCore.iOS
{
    internal enum PHAuthorizationStatus : long
    {
        PHAuthorizationStatusNotDetermined,

        PHAuthorizationStatusRestricted,

        PHAuthorizationStatusDenied,

        PHAuthorizationStatusAuthorized,
    }
}
#endif