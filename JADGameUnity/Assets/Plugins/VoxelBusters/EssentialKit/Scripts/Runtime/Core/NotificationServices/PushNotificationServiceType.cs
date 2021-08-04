using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Push notification service type.
    /// </summary>
    public enum PushNotificationServiceType
    {
        /// <summary> Undefined.</summary>
        None = 0,

        /// <summary> Service type which is not yet supported by plugin.</summary>
        Custom,

        /// <summary> Uses one signal service.</summary>
        OneSignal,
    }
}