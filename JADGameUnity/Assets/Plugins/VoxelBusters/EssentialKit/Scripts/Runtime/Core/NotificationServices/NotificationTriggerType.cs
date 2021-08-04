using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Constants indicating available trigger types.
    /// </summary>
    [Flags]
    public enum NotificationTriggerType
    {
        Undefined,

        /// <summary> Triggers notification after the specified amount of time elapses. </summary>
        TimeInterval        = 1 << 0,

        /// <summary> Triggers notification at a specific date and time. </summary>
        Calendar            = 1 << 1,

        /// <summary> Triggers notification after the user's device enters or exits the specified geographic region. </summary>
        Location            = 1 << 2,

        /// <summary> Notification received from Push Notification Service. </summary>
        PushNotification    = 1 << 3,

        LocalNotification   = (TimeInterval | Calendar | Location),
    }
}