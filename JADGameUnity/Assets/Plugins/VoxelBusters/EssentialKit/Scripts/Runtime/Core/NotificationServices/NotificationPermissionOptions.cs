using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Enumeration values for requesting authorization to interact with the user.
    /// </summary>
    [Flags]
    public enum NotificationPermissionOptions
    {
        None                            = 0,

        /// <summary> The ability to update the app’s badge. </summary>
        Badge                           = 1 << 0,

        /// <summary> The ability to play sounds. </summary>
        Sound                           = 1 << 1,

        /// <summary> The ability to display alerts. </summary>
        Alert                           = 1 << 2,

        /// <summary> The ability to display notifications in a CarPlay environment. </summary>
        CarPlay                         = 1 << 3,

        /// <summary> The ability to play sounds for critical alerts. </summary>
        CriticalAlert                   = 1 << 4,

        /// <summary> An option indicating the system should display a button for in-app notification settings. </summary>
        ProvidesAppNotificationSettings = 1 << 5,

        /// <summary> The ability to post noninterrupting notifications provisionally to the Notification Center. </summary>
        Provisional                     = 1 << 6,

        /// <summary> The ability for Siri to automatically read out messages over AirPods. </summary>
        Announcement                    = 1 << 7,

        All                             = Badge | Sound | Alert | CarPlay | CriticalAlert | ProvidesAppNotificationSettings | Provisional | Announcement,
    }
}