using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Constants indicating whether the app is allowed to schedule notifications.
    /// </summary>
    public enum NotificationPermissionStatus
    {
        /// <summary> The user hasn't yet made a choice about whether the app is allowed to schedule notifications. </summary>
        NotDetermined,

        /// <summary> The app isn't authorized to schedule or receive notifications. </summary>
        Denied,

        /// <summary> The app is authorized to schedule or receive notifications. </summary>
        Authorized,

        /// <summary> The application is provisionally authorized to post noninterruptive user notifications. </summary>
        Provisional,
    }
}