using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// The period of time to which user's best score are restricted.
    /// </summary>
    public enum LeaderboardTimeScope
    {
        /// <summary> Best score of all user's recorded in past 24hrs is returned. </summary>
        Today,

        /// <summary> Best score of all user's recorded in past week is returned. </summary>
        Week,

        /// <summary> Best score of all user's recorded is returned. </summary>
        AllTime,
    }
}