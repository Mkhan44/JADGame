using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information related to <see cref="CloudServices.OnUserChange"/> event.
    /// </summary>
    public class CloudServicesUserChangeResult
    {
        #region Properties

        /// <summary>
        /// The cloud user.
        /// </summary>
        public ICloudUser User
        {
            get;
            internal set;
        }

        #endregion
    }
}