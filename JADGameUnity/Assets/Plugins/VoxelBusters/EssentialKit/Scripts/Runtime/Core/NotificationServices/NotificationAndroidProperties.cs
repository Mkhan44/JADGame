using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Notification properties specific to Android platform.
    /// </summary>
    public class NotificationAndroidProperties
    {
        #region Properties

        /// <summary>
        /// The tag of the notification.
        /// </summary>
        /// <description>
        /// The tag defines this notification uniquely or can be empty which overwrites previous notification. 
        /// If the tag is set with different value than previous notification, it won't override the previous one in notification bar, otherwise it will.
        /// </description>
        public string Tag
        {
            get; 
            set;
        }

        /// <summary>
        /// The image used as the large icon for notification.
        /// </summary>
        /// <remarks>
        /// \note This will be the icon thats displayed in the notification. 
        /// If the value is not set, then default image will be used. 
        /// </remarks>
        public string LargeIcon
        {
            get; 
            set;
        }


        /// <summary>
        /// The image used as the big picture for notification.
        /// </summary>
        /// <remarks>
        /// \note This will be the image used as the preview for notification.
        /// </remarks>
        public string BigPicture
        {
            get;
            set;
        }

        #endregion
    }
}