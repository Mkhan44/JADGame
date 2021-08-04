using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Constants indicating the style previewing a notification's content.
    /// </summary>
    public enum NotificationPreviewStyle
    {
        /// <summary> The notification's content is always shown, even when the device is locked. </summary>
        Always = 0,

        /// <summary> The notification's content is shown only when the device is unlocked. </summary>
        WhenAuthenticated,

        /// <summary> The notification's content is never shown, even when the device is unlocked. </summary>
        Never,

        NotAccessible
    }
}