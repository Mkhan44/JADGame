using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit.NotificationServicesCore;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Builder class for <see cref="INotification"/> objects. 
    /// Provides a convenient way to set the various fields of a <see cref="INotification"/>.
    /// </summary>
    public class NotificationBuilder
    {
        #region Fields

        private     IMutableNotification    m_notification;

        #endregion

        #region Constructors

        private NotificationBuilder()
        { }

        #endregion

        #region Create methods

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationBuilder"/> class.
        /// </summary>
        /// <param name="notificationId">The unique identifier for this notification.</param>
        public static NotificationBuilder CreateNotification(string notificationId)
        {
            var     nativeInterface     = NotificationServices.NativeInterface;
            return new NotificationBuilder()
            {
                m_notification          = nativeInterface.CreateMutableNotification(notificationId),
            };
        }

        #endregion

        #region Setter methods

        /// <summary>
        /// Sets the title property of notification.
        /// </summary>
        /// <param name="value">Value.</param>
        public NotificationBuilder SetTitle(string value)
        {
            // validate arguments
            if (null == value)
            {
                DebugLogger.LogWarning("Title value is null.");
                return this;
            }

            // set value
            m_notification.SetTitle(value);

            return this;
        }

        /// <summary>
        /// Sets the subtitle property of notification.
        /// </summary>
        /// <param name="value">Value.</param>
        public NotificationBuilder SetSubtitle(string value)
        {
            // validate arguments
            if (null == value)
            {
                DebugLogger.LogWarning("Subtitle value is null.");
                return this;
            }

            // set value
            m_notification.SetSubtitle(value);

            return this;
        }

        /// <summary>
        /// Sets the body property of notification.
        /// </summary>
        /// <param name="value">Value.</param>
        public NotificationBuilder SetBody(string value)
        {
            // validate arguments
            if (null == value)
            {
                DebugLogger.LogWarning("Body value is null.");
                return this;
            }

            // set value
            m_notification.SetBody(value);

            return this;
        }

        /// <summary>
        /// Sets the badge property of notification.
        /// </summary>
        /// <param name="value">Value.</param>
        public NotificationBuilder SetBadge(int value)
        {
            // validate arguments
            if (value < 0)
            {
                DebugLogger.LogWarning("Badge value must be a positive number.");
                return this;
            }

            // set value
            m_notification.SetBadge(value);

            return this;
        }

        /// <summary>
        /// Sets the custom userinfo property of notification.
        /// </summary>
        /// <param name="value">Value.</param>
        public NotificationBuilder SetUserInfo(Dictionary<string, string> value)
        {
            // validate arguments
            if (null == value)
            {
                DebugLogger.LogWarning("User info value is null.");
                return this;
            }

            // set value
            m_notification.SetUserInfo(value);

            return this;
        }

        /// <summary>
        /// Sets the custom userinfo property of notification.
        /// </summary>
        /// <param name="value">Value.</param>
        public NotificationBuilder SetUserInfo(params KeyValuePair<string, string>[] values)
        {
            // validate arguments
            if (null == values)
            {
                DebugLogger.LogWarning("User info value is null.");
                return this;
            }


            // set value
            var     userInfo    = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> keyValuePair in values)
            {
                userInfo.Add(keyValuePair.Key, keyValuePair.Value);
            }

            m_notification.SetUserInfo(userInfo);

            return this;
        }

        /// <summary>
        /// Sets the sound filename property of notification.
        /// </summary>
        /// <param name="value">Value.</param>
        public NotificationBuilder SetSoundFileName(string value)
        {
            // validate arguments
            if (null == value)
            {
                DebugLogger.LogWarning("Sound file name value is null.");
                return this;
            }

            // set value
            m_notification.SetSoundFileName(value);

            return this;
        }

        /// <summary>
        /// Sets the iOS specific properties of notification.
        /// </summary>
        /// <param name="value">Value.</param>
        public NotificationBuilder SetIosProperties(NotificationIosProperties value)
        {
            // validate arguments
            if (null == value)
            {
                DebugLogger.LogWarning("iOS properties value is null.");
                return this;
            }

            // set value
            m_notification.SetIosProperties(value);

            return this;
        }

        /// <summary>
        /// Sets the android specific properties of notification.
        /// </summary>
        /// <param name="value">Value.</param>
        public NotificationBuilder SetAndroidProperties(NotificationAndroidProperties value)
        {
            // validate arguments
            if (null == value)
            {
                DebugLogger.LogWarning("Android properties value is null.");
                return this;
            }

            // set value
            m_notification.SetAndroidProperties(value);

            return this;
        }

        /// <summary>
        /// Adds the time interval based trigger.
        /// </summary>
        /// <param name="interval">The time (in seconds) that must elapse from the current time before the trigger fires.</param>
        /// <param name="repeats">If set to <c>true</c> repeats.</param>
        public NotificationBuilder SetTimeIntervalNotificationTrigger(double interval, bool repeats = false)
        {
            // validate arguments
            if (interval < 0)
            {
                DebugLogger.LogWarning("Interval value must be a positive number.");
                return this;
            }

            // create trigger object
            var     trigger     = new TimeIntervalNotificationTrigger(interval, repeats);

            // add trigger
            m_notification.SetTrigger(trigger);

            return this;
        }

        /// <summary>
        /// Adds the date time based trigger.
        /// </summary>
        /// <returns>The date time based trigger.</returns>
        /// <param name="dateComponent">The time when notification is triggered for first time.</param>
        /// <param name="repeats">If set to <c>true</c> repeats.</param>
        public NotificationBuilder SetCalendarNotificationTrigger(DateComponents dateComponent, bool repeats = false)
        {
            // validate arguments
            if (null == dateComponent)
            {
                DebugLogger.LogWarning("Date component value is null.");
                return this;
            }

            // create trigger object
            var     trigger     = new CalendarNotificationTrigger(dateComponent, repeats);

            // add trigger
            m_notification.SetTrigger(trigger);

            return this;
        }

        /// <summary>
        /// Adds the location based trigger.
        /// </summary>
        /// <returns>The location based trigger.</returns>
        /// <param name="region">The geographic region that must be entered or exited.</param>
        /// <param name="notifyOnEntry">If set to <c>true</c> notify on entry.</param>
        /// <param name="notifyOnExit">If set to <c>true</c> notify on exit.</param>
        /// <param name="repeats">If set to <c>true</c> repeats.</param>
        public NotificationBuilder SetLocationNotificationTrigger(CircularRegion region, bool notifyOnEntry, bool notifyOnExit, bool repeats = false)
        {
            // create trigger object
            var     trigger     = new LocationNotificationTrigger(region, notifyOnEntry, notifyOnExit, repeats);

            // add trigger
            m_notification.SetTrigger(trigger);

            return this;
        }

        /// <summary>
        /// Returns newly created <see cref="IMutableNotification"/> instance.
        /// </summary>
        public INotification Create()
        {
            try
            {
                return m_notification;
            }
            finally
            {
                m_notification  = null;
            }
        }

        #endregion
    }
}