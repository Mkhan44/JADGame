using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// A trigger condition that indicates the notification was using a Push Notification Service.
    /// </summary>
    [Serializable]
    public sealed class PushNotificationTrigger : INotificationTrigger
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PushNotificationTrigger"/> class.
        /// </summary>
        public PushNotificationTrigger()
        {
            // set properties
            Repeats = false;
        }

        #endregion

        #region INotificationTrigger implementation

        public bool Repeats 
        { 
            get; 
            private set; 
        }

        #endregion
    }
}