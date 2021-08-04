#if UNITY_ANDROID
using System;
using System.Collections;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Android
{
    internal class Notification : NotificationBase
    {
        #region Fields

        private NativeNotification              m_instance;
        private INotificationTrigger            m_trigger;
        private NotificationAndroidProperties   m_androidProperties;

        #endregion

        #region Constructors

        protected Notification(string notificationId)
            : base(notificationId)
        {
            m_instance = new NativeNotification(notificationId);
        }

        public Notification(NativeNotification nativeNotification)
            : base(nativeNotification.Id)
        {
            m_instance = nativeNotification;
        }

        ~Notification()
        {
            Dispose(false);
        }

        #endregion

        #region Base class implementation

        protected override string GetTitleInternal()
        {
            return m_instance.ContentTitle;
        }

        protected override string GetSubtitleInternal()
        {
            return "";
        }

        protected override string GetBodyInternal()
        {
            return m_instance.ContentText;
        }

        protected override int GetBadgeInternal()
        {
            return m_instance.Badge;
        }

        protected override IDictionary GetUserInfoInternal()
        {
            if (m_instance.UserInfo != null)
            {
                string jsonString = m_instance.UserInfo.ToString();
                IDictionary data = (IDictionary)ExternalServiceProvider.JsonServiceProvider.FromJson(jsonString);
                return data;
            }
            else
            {
                return null;
            }
        }

        protected override string GetSoundFileNameInternal()
        {
            return m_instance.SoundFileName;
        }

        protected override INotificationTrigger GetTriggerInternal()
        {
            if(m_trigger == null)
            {
                NativeNotificationTrigger trigger = m_instance.Trigger;
                if (trigger != null)
                {
                    if(m_instance.HasDateTimeTrigger())
                    {
                        NativeTimeIntervalNotificationTrigger dateTimeTrigger = new NativeTimeIntervalNotificationTrigger(trigger);
                        m_trigger = new TimeIntervalNotificationTrigger(dateTimeTrigger.TimeInterval, dateTimeTrigger.Repeat, dateTimeTrigger.GetNextTriggerDate().GetDateTime());
                    }
                    else if(m_instance.HasLocationTrigger())
                    {
                        NativeLocationNotificationTrigger locationNotificationTrigger = new NativeLocationNotificationTrigger(trigger);

                        LocationCoordinate coordinate = new LocationCoordinate()
                        {
                            Latitude = locationNotificationTrigger.GetLocationCoordinate().GetX(),
                            Longitude = locationNotificationTrigger.GetLocationCoordinate().GetY()
                        };

                        CircularRegion circularRegion = new CircularRegion();
                        circularRegion.Center = coordinate;
                        circularRegion.Radius = locationNotificationTrigger.GetRadius();
                        DebugLogger.LogWarning("Ignoring regionId on Android");
                        m_trigger = new LocationNotificationTrigger(circularRegion, locationNotificationTrigger.IsNotifyOnEntry(), locationNotificationTrigger.IsNotifyOnExit(), locationNotificationTrigger.Repeat);
                    }
                }
            }

            return m_trigger;
        }

        protected override NotificationIosProperties GetIosPropertiesInternal()
        {
            return null;
        }

        protected override NotificationAndroidProperties GetAndroidPropertiesInternal()
        {
            if(m_androidProperties != null)
            {
                m_androidProperties = new NotificationAndroidProperties();
                m_androidProperties.Tag = m_instance.Tag;
                m_androidProperties.LargeIcon = m_instance.LargeIcon;
                m_androidProperties.BigPicture = m_instance.BigPicture;
            }
           
            return m_androidProperties;
        }

        #endregion
    }
}
#endif