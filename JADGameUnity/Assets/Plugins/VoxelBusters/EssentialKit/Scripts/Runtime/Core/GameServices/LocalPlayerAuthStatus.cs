using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Enumeration for determining <see cref="ILocalPlayer"/> auth status.
    /// </summary>
    public enum LocalPlayerAuthStatus
    {
        /// <summary> User authentication status is not found.</summary>
        NotAvailable   = 0,

        /// <summary> Local player auth process has been initiated.</summary>
        Authenticating,

        /// <summary> Local player is signed in.</summary>
        Authenticated
    }
}