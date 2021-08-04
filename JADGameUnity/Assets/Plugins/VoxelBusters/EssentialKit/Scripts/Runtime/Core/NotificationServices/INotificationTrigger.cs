using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Base interface for representing an event that triggers the delivery of a notification.
    /// </summary>
    public interface INotificationTrigger
    {
        #region Properties

        /// <summary>
        /// A Boolean value indicating whether the system reschedules the notification after it is delivered.
        /// </summary>
        bool Repeats
        {
            get;
        }

        #endregion
    }
}