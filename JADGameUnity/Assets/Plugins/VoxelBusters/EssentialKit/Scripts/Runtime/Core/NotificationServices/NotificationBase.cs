using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NotificationServicesCore
{
    public abstract class NotificationBase : NativeObjectBase, INotification
    {
        #region Fields

        [SerializeField]
        private     string          m_id;

        #endregion

        #region Constructors

        protected NotificationBase(string id)
        {
            // set properties
            m_id    = id;
        }

        #endregion

        #region Abstract methods

        protected abstract string GetTitleInternal();

        protected abstract string GetSubtitleInternal();

        protected abstract string GetBodyInternal();

        protected abstract int GetBadgeInternal();

        protected abstract IDictionary GetUserInfoInternal();

        protected abstract string GetSoundFileNameInternal();
        
        protected abstract INotificationTrigger GetTriggerInternal();
        
        protected abstract NotificationIosProperties GetIosPropertiesInternal();
        
        protected abstract NotificationAndroidProperties GetAndroidPropertiesInternal();

        #endregion

        #region Base class methods

        public override string ToString()
        {
            var     sb  = new StringBuilder();
            sb.Append("Notification { ");
            sb.Append("Id: ").Append(Id).Append(" ");
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region INotification implementation

        public string Id
        {
            get
            {
                return m_id;
            }
        }

        public string Title
        {
            get
            {
                return GetTitleInternal();
            }
        }

        public string Subtitle
        {
            get
            {
                return GetSubtitleInternal();
            }
        }

        public string Body
        {
            get
            {
                return GetBodyInternal();
            }
        }

        public int Badge
        {
            get
            {
                return GetBadgeInternal();
            }
        }

        public IDictionary UserInfo
        {
            get
            {
                return GetUserInfoInternal();
            }
        }

        public string SoundFileName
        {
            get
            {
                return GetSoundFileNameInternal();
            }
        }

        public NotificationTriggerType TriggerType
        {
            get
            {
                INotificationTrigger    trigger     = Trigger;
                if (trigger is TimeIntervalNotificationTrigger)
                {
                    return NotificationTriggerType.TimeInterval;
                }
                if (trigger is CalendarNotificationTrigger)
                {
                    return NotificationTriggerType.Calendar;
                }
                if (trigger is LocationNotificationTrigger)
                {
                    return NotificationTriggerType.Location;
                }
                if (trigger is PushNotificationTrigger)
                {
                    return NotificationTriggerType.PushNotification;
                }

                return NotificationTriggerType.Undefined;
            }
        }

        public INotificationTrigger Trigger
        {
            get
            {
                return GetTriggerInternal();
            }
        }

        public NotificationIosProperties IosProperties
        {
            get
            {
                return GetIosPropertiesInternal();
            }
        }

        public NotificationAndroidProperties AndroidProperties
        {
            get
            {
                return GetAndroidPropertiesInternal();
            }
        }

        #endregion
    }
}