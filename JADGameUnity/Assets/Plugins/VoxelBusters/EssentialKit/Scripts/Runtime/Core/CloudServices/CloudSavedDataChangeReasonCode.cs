using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Possible reasons when cloud data changed event occurs.
    /// </summary>
    public enum CloudSavedDataChangeReasonCode
    {
        /// <summary> This occurs when another instance of your app using same cloud service account, uploads a new value. </summary>
        ServerChange,

        /// <summary> This occurs when an attempt to write to key-value storage was discarded because an initial download from cloud server has not yet happened.</summary>
        InitialSyncChange,

        /// <summary> This occurs when your app’s key-value store has exceeded its space quota on the cloud server.</summary>
        QuotaViolationChange,

        /// <summary> This occurs when user has changed the cloud service account. The keys and values in the local key-value store have been replaced with those from the new account.</summary>
        AccountChange
    }
}