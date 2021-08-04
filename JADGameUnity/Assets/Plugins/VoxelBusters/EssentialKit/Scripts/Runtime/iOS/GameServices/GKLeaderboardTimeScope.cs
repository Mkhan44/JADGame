#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.GameServicesCore.iOS
{
    internal enum GKLeaderboardTimeScope : long
    {
        GKLeaderboardTimeScopeToday = 0,

        GKLeaderboardTimeScopeWeek,

        GKLeaderboardTimeScopeAllTime
    }
}
#endif