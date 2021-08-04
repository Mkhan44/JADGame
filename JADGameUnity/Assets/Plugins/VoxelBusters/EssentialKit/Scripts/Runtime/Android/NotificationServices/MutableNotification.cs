#if UNITY_ANDROID
using System;
using System.Collections;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    internal sealed class MutableNotification : Notification, IMutableNotification
    {

        #region Fields

        private NativeNotificationBuilder m_instance;

        #endregion

        #region Constructors

        public MutableNotification(string notificationId)
            : base(notificationId)
        {
            m_instance = new NativeNotificationBuilder(notificationId);
        }

        ~MutableNotification()
        {
            Dispose(false);
        }

        #endregion

        #region INotificationPropertySetter implementation

        public void SetTitle(string value)
        {
            m_instance.SetTitle(value);
        }

        public void SetSubtitle(string value)
        {
            //No operation
            DebugLogger.LogWarning("This value is ignored on this platform");
        }

        public void SetBody(string value)
        {
            m_instance.SetBody(value);
        }

        public void SetBadge(int value)
        {
            m_instance.SetBadge(value);
        }

        public void SetUserInfo(IDictionary value)
        {
            string jsonString = ExternalServiceProvider.JsonServiceProvider.ToJson(value);
            m_instance.SetUserInfo(jsonString);
        }
        
        public void SetSoundFileName(string value)
        {
            m_instance.SetSoundFileName(value);
        }
        
        public void SetIosProperties(NotificationIosProperties value)
        { }
        
        public void SetAndroidProperties(NotificationAndroidProperties value)
        {
            m_instance.SetLargeIcon(value.LargeIcon);
            m_instance.SetBigPicture(value.BigPicture);
        }

        public void SetTrigger(INotificationTrigger trigger)
        {
            if(trigger is TimeIntervalNotificationTrigger)
            {
                TimeIntervalNotificationTrigger timeTrigger = (TimeIntervalNotificationTrigger)trigger;
                NativeTimeIntervalNotificationTrigger nativeTrigger = new NativeTimeIntervalNotificationTrigger((long)(timeTrigger.TimeInterval * 1000), timeTrigger.Repeats);
                m_instance.SetTrigger(nativeTrigger);
            }
            /*else if (trigger is CalendarNotificationTrigger)
            {
                CalendarNotificationTrigger calenderTrigger = (CalendarNotificationTrigger)trigger;
                NativeCalendarNotificationTrigger nativeTrigger = new NativeCalendarNotificationTrigger();
                m_instance.SetTrigger(nativeTrigger);
            }*/
            else if(trigger is LocationNotificationTrigger)
            {
                LocationNotificationTrigger locationTrigger = (LocationNotificationTrigger)trigger;
                NativeLocationNotificationTrigger nativeTrigger = new NativeLocationNotificationTrigger(locationTrigger.Region.Center.Latitude, locationTrigger.Region.Center.Longitude, locationTrigger.Region.Radius, locationTrigger.Repeats);
                nativeTrigger.SetNotifyOnEntry(locationTrigger.NotifyOnEntry);
                nativeTrigger.SetNotifyOnExit(locationTrigger.NotifyOnExit);
                m_instance.SetTrigger(nativeTrigger);
            }
            else
            {
                throw VBException.NotImplemented("Not implemented on Android : " + trigger.GetType());
            }
        }

        public NativeNotification Build()
        {
            return m_instance.Build();
        }

        #endregion
    }
}
#endif