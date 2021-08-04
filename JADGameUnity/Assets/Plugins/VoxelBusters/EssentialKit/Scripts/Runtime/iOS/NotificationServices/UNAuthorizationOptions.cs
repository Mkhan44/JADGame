#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{
    [Flags]
    internal enum UNAuthorizationOptions : long
    {
        UNAuthorizationOptionBadge                              = (1 << 0),

        UNAuthorizationOptionSound                              = (1 << 1),

        UNAuthorizationOptionAlert                              = (1 << 2),

        UNAuthorizationOptionCarPlay                            = (1 << 3),

        UNAuthorizationOptionCriticalAlert                      = (1 << 4),

        UNAuthorizationOptionProvidesAppNotificationSettings    = (1 << 5),

        UNAuthorizationOptionProvisional                        = (1 << 6),

        UNAuthorizationOptionAnnouncement                       = (1 << 7)
    }
}
#endif