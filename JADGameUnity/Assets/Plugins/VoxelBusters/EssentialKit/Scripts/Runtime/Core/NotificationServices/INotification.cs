using System.Collections;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides a cross-platform interface to access properties of Notification object.
    /// </summary>
    public interface INotification
    {
        #region Properties

        /// <summary>
        /// The unique identifier for this notification.
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// The short description of the notification.
        /// </summary>
        string Title
        {
            get;
        }

        /// <summary>
        /// The secondary description of the notification.
        /// </summary>
        string Subtitle
        {
            get;
        }

        /// <summary>
        /// The message included in the notification.
        /// </summary>
        string Body
        {
            get;
        }

        /// <summary>
        /// The number to display as the app’s icon badge.
        /// </summary>
        int Badge
        {
            get; 
        }

        /// <summary>
        /// A dictionary of custom information associated with the notification.
        /// </summary>
        IDictionary UserInfo
        {
            get;
        }

        /// <summary>
        /// The sound to play when the notification is delivered.
        /// </summary>
        string SoundFileName
        {
            get;
        }

        /// <summary>
        /// The type of the trigger associated with the notification.
        /// </summary>
        NotificationTriggerType TriggerType
        {
            get;
        }

        /// <summary>
        /// The trigger associated with the notification.
        /// </summary>
        INotificationTrigger Trigger
        {
            get;
        }

        /// <summary>
        /// The object containing properties specific to android.
        /// </summary>
        /// <value>The android properties.</value>
        NotificationIosProperties IosProperties
        {
            get;
        }

        /// <summary>
        /// The object containing properties specific to android.
        /// </summary>
        /// <value>The android properties.</value>
        NotificationAndroidProperties AndroidProperties
        {
            get;
        }

        #endregion
    }
}