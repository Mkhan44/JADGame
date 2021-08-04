using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Simulator
{
    public sealed class MutableNotification : Notification, IMutableNotification 
    {
        #region Constructors

        public MutableNotification(string notificationId)
            : base(notificationId)
        {
            //
        }

        ~MutableNotification()
        {
            Dispose(false);
        }

        #endregion

        #region IMutableNotification implementation

        public void SetTitle(string value)
        {
            // set value
            m_title         = value;
        }

        public void SetSubtitle(string value)
        {
            // set value
            m_subtitle      = value;
        }

        public void SetBody(string value)
        {
            // set value
            m_body          = value;
        }

        public void SetBadge(int value)
        {
            // set value
            m_badge         = value;
        }

        public void SetUserInfo(IDictionary value)
        {
            // set value
            m_userInfo      = ExternalServiceProvider.JsonServiceProvider.ToJson(value);
        }
        
        public void SetSoundFileName(string value)
        {
            // set value
            m_sound         = value;
        }
        
        public void SetIosProperties(NotificationIosProperties value)
        { }
        
        public void SetAndroidProperties(NotificationAndroidProperties value)
        { }

        public void SetTrigger(INotificationTrigger trigger)
        {
            Assertions.AssertIfArgIsNull(trigger, "trigger");

            if (m_triggerType != NotificationTriggerType.Undefined)
            {
                return;
            }

            // set trigger data
            if (trigger is TimeIntervalNotificationTrigger)
            {
                m_triggerType   = NotificationTriggerType.TimeInterval;
            }
            if (trigger is CalendarNotificationTrigger)
            {
                m_triggerType   = NotificationTriggerType.Calendar;
            }
            if (trigger is LocationNotificationTrigger)
            {
                m_triggerType   = NotificationTriggerType.Location;
            }
            m_triggerData       = JsonUtility.ToJson(trigger);
        }

        #endregion
    }
}