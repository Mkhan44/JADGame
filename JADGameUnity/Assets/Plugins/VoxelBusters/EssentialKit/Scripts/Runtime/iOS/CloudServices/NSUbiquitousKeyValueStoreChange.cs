#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.CloudServicesCore.iOS
{
    internal enum NSUbiquitousKeyValueStoreChange : long
    {
        NSUbiquitousKeyValueStoreServerChange               = 0,

        NSUbiquitousKeyValueStoreInitialSyncChange          = 1,

        NSUbiquitousKeyValueStoreQuotaViolationChange       = 2,

        NSUbiquitousKeyValueStoreAccountChange              = 3,
    }
}
#endif