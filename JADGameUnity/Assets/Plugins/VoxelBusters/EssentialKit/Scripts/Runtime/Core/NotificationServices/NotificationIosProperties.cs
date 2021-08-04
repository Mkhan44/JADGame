using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Notification properties specific to iOS platform.
    /// </summary>
    public class NotificationIosProperties
    {
        #region Properties

        /// <summary>
        /// The name of the launch image to display when your app is launched in response to the notification
        /// </summary>
        public string LaunchImageFileName
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public NotificationIosProperties(string launchImageFileName = null)
        {
            // set properties
            LaunchImageFileName     = launchImageFileName;
        }

        #endregion
    }
}