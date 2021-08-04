#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{
    [Flags]
    internal enum UNNotificationPresentationOptions : long 
    {
        UNNotificationPresentationOptionBadge   = (1 << 0),

        UNNotificationPresentationOptionSound   = (1 << 1),

        UNNotificationPresentationOptionAlert   = (1 << 2),
    }
}
#endif