using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Constants indicating how to present a notification in a foreground app.
    /// </summary>
    [Flags]
    public enum NotificationPresentationOptions
    {
        /// <summary> Display the alert using the content provided by the notification. </summary>
        Alert   = 1 << 0,

        /// <summary> Apply the notification's badge value to the app’s icon. </summary>
        Badge   = 1 << 1,

        /// <summary> Play the sound associated with the notification. </summary>
        Sound   = 1 << 2,
    }
}