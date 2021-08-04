using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Result codes returned when the game service interface is dismissed.
    /// </summary>
    public enum GameServicesViewResultCode
    {
        /// <summary> The user action could not be determined.  This occurs in platforms where there is no provision to find result. </summary>
        Unknown,

        /// <summary> The user successfully closed. </summary>
        Done,
    }
}