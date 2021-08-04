#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{
    internal enum UNAlertStyle : long
    {
        UNAlertStyleNone = 0,

        UNAlertStyleBanner,

        UNAlertStyleAlert,
    }
}
#endif