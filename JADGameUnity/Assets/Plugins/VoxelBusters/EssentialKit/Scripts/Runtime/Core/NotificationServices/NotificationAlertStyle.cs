using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Constants indicating the presentation styles for alerts.
    /// </summary>
    public enum NotificationAlertStyle
    {
        /// <summary> No alert. </summary>
        None = 0,

        /// <summary> Modal alerts. </summary>
        Alert,

        /// <summary> Banner alerts. </summary>
        Banner,
    }
}