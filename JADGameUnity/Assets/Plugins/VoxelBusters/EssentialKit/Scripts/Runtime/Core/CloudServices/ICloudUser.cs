using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides a cross-platform interface to access information related to cloud user.
    /// </summary>
    public interface ICloudUser
    {
        #region Properties

        /// <summary>
        /// The string to identify active user.
        /// </summary>
        string UserId
        {
            get;
        }

        /// <summary>
        /// The current status of user account.
        /// </summary>
        CloudUserAccountStatus AccountStatus
        {
            get;
        }

        #endregion
    }
}