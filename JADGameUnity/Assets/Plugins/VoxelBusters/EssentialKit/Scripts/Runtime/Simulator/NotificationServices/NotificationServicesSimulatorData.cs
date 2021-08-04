using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Simulator
{
    [Serializable]
    internal sealed class NotificationServicesSimulatorData 
    {
        #region Nested types

        [SerializeField]
        private         NotificationPermissionStatus        m_permissionStatus                      = NotificationPermissionStatus.NotDetermined;

        [SerializeField]
        private         NotificationPermissionOptions       m_enabledOptions                        = NotificationPermissionOptions.None;

        [SerializeField]
        private         List<Notification>                  m_scheduledNotifications                = new List<Notification>();

        [SerializeField]
        private         List<Notification>                  m_deliveredNotifications                = new List<Notification>();

        [SerializeField]
        private         bool                                m_isRegistedForRemoteNotifications      = false;

        #endregion

        #region Properties

        public NotificationPermissionStatus PermissionStatus
        {
            get
            {
                return m_permissionStatus;
            }
        }
        
        public NotificationPermissionOptions EnabledOptions
        {
            get
            {
                return m_enabledOptions;
            }
        }

        public bool IsRegistedForRemoteNotifications
        {
            get
            {
                return m_isRegistedForRemoteNotifications;
            }
            set
            {
                m_isRegistedForRemoteNotifications  = value;
            }
        }

        #endregion

        #region Public methods

        public void SetPermissionStatus(NotificationPermissionStatus permissionStatus, NotificationPermissionOptions enabledOptions)
        {
            // set values
            m_permissionStatus  = permissionStatus;
            m_enabledOptions    = enabledOptions;
        }

        public void AddScheduledNotification(Notification notification)
        {
            int index;
            // replace existing objects
            if (-1 != (index = FindScheduledNotificationWithId(notification.Id)))
            {
                m_scheduledNotifications[index] = notification;
            }
            else
            {
                m_scheduledNotifications.Add(notification);
            }
        }

        public IEnumerable<Notification> GetScheduledNotifications()
        {
            return m_scheduledNotifications;
        }

        public void RemoveNotificationWithId(Notification notification)
        {
            int index;
            if (-1 != (index = FindScheduledNotificationWithId(notification.Id)))
            {
                m_scheduledNotifications.RemoveAt(index);
            }
        }

        public void RemoveAllScheduledNotifications()
        {
            m_scheduledNotifications.Clear();
        }

        private int FindScheduledNotificationWithId(string id)
        {
            return m_scheduledNotifications.FindIndex((item) => string.Equals(id, item.Id));
        }

        public IEnumerable<Notification> GetDeliveredNotifications()
        {
            return m_deliveredNotifications;
        }

        public void ClearDeliveredNotifications()
        {
            m_deliveredNotifications.Clear();
        }

        #endregion
    }
}