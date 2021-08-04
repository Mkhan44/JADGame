using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Enumeration indicating the availability of the user’s cloud account.
    /// </summary>
    public enum CloudUserAccountStatus
    {
        /// <summary> Indicates that an error occurred during an attempt to retrieve the account status. </summary>
        CouldNotDetermine    = 0,

        /// <summary> The user’s iCloud account is available and may be used by this app. </summary>
        Available            = 1,

        /// <summary> The user’s iCloud account is not available. Access was denied due to Parental Controls. </summary>
        Restricted           = 2,

        /// <summary> The user’s iCloud account is not available because no account information has been provided for this device. </summary>
        NoAccount            = 3,
    }
}