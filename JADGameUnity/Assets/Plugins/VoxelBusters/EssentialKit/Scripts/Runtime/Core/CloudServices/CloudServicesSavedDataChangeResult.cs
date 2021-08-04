using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// This class contains the information related to <see cref="CloudServices.OnSavedDataChange"/> event.
    /// </summary>
    public class CloudServicesSavedDataChangeResult
    {
        #region Properties

        /// <summary>
        /// The reason causing local data change.
        /// </summary>
        public CloudSavedDataChangeReasonCode ChangeReason
        {
            get;
            internal set;
        }

        /// <summary>
        /// An array of changed keys.
        /// </summary>
        public string[] ChangedKeys
        {
            get;
            internal set;
        }

        #endregion
    }
}