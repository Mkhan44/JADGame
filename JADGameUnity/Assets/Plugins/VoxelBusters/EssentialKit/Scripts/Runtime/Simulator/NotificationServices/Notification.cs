using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Simulator
{
    [Serializable]
    public class Notification : NotificationBase
    {
        #region Fields

        [SerializeField]
        protected       string                          m_title;

        [SerializeField]
        protected       string                          m_subtitle;

        [SerializeField]
        protected       string                          m_body;

        [SerializeField]
        protected       int                             m_badge;

        [SerializeField]
        protected       string                          m_userInfo;

        [SerializeField]
        protected       string                          m_sound;

        [SerializeField]
        protected       NotificationTriggerType         m_triggerType           = NotificationTriggerType.Undefined;

        [SerializeField]
        protected       string                          m_triggerData;
        
        #endregion

        #region Constructors

        public Notification(string id) : base(id)
        { }

        ~Notification()
        {
            Dispose(false);
        }

        #endregion

        #region Base class implementation

        protected override string GetTitleInternal()
        {
            return m_title;
        }

        protected override string GetSubtitleInternal()
        {
            return m_subtitle;
        }

        protected override string GetBodyInternal()
        {
            return m_body;
        }

        protected override int GetBadgeInternal()
        {
            return m_badge;
        }

        protected override IDictionary GetUserInfoInternal()
        {
            return string.IsNullOrEmpty(m_userInfo) 
                ? null 
                : (IDictionary)ExternalServiceProvider.JsonServiceProvider.FromJson(m_userInfo);
        }

        protected override string GetSoundFileNameInternal()
        {
            return m_sound;
        }

        protected override INotificationTrigger GetTriggerInternal()
        {
            switch (m_triggerType)
            {
                case NotificationTriggerType.Undefined:
                    return null;

                case NotificationTriggerType.TimeInterval:
                    return JsonUtility.FromJson<TimeIntervalNotificationTrigger>(m_triggerData);

                case NotificationTriggerType.Calendar:
                    return JsonUtility.FromJson<CalendarNotificationTrigger>(m_triggerData);

                case NotificationTriggerType.LocalNotification:
                    return JsonUtility.FromJson<LocationNotificationTrigger>(m_triggerData);

                case NotificationTriggerType.PushNotification:
                    return JsonUtility.FromJson<PushNotificationTrigger>(m_triggerData);

                default:
                    throw VBException.SwitchCaseNotImplemented(m_triggerType);
            }
        }

        protected override NotificationIosProperties GetIosPropertiesInternal()
        {
            return null;
        }

        protected override NotificationAndroidProperties GetAndroidPropertiesInternal()
        {
            return null;
        }

        #endregion
    }
}