using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// The scope of player to be searched for scores.
    /// </summary>
    public enum LeaderboardPlayerScope
    {
        /// <summary> All the players are considered for search. </summary>
        Global,

        /// <summary> Only friends of local player are considered for search. </summary>
        FriendsOnly,
    }
}