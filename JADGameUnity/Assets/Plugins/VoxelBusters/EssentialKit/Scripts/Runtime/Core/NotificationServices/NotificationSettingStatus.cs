using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Enumeration values indicating the current status of a notification setting.
    /// </summary>
    [Flags]
    public enum NotificationSettingStatus
    {
        /// <summary> The notification setting is turned off. </summary>
        Disabled,

        /// <summary> The notification setting is turned on. </summary>
        Enabled,

        /// <summary> The app does not support this notification setting. </summary>
        NotSupported,

        /// <summary> The platform unable to fetch this setting. </summary>
        NotAccessible
    }
}