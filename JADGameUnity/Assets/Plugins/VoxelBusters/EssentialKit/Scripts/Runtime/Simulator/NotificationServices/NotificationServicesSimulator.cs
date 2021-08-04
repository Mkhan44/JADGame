using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.Editor.NativePlugins;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.Simulator
{
    public sealed class NotificationServicesSimulator : SingletonObject<NotificationServicesSimulator>
    {
        #region Constants

        // messages
        private     const       string              kUnauthorizedAccessError    = "Unauthorized access! Check permission before accessing notification feature.";
            
        private     const       string              kAlreadyAuthorizedError     = "Permission for accessing notification is already provided.";

        #endregion

        #region properties

        private     NotificationServicesSimulatorData   m_simulatorData;

        #endregion

        #region Constructors

        private NotificationServicesSimulator()
        {
            // save instance
            m_simulatorData     = LoadFromDisk() ?? new NotificationServicesSimulatorData();
        }

        #endregion

        #region Database methods

        private NotificationServicesSimulatorData LoadFromDisk()
        {
            return SimulatorDatabase.Instance.GetObject<NotificationServicesSimulatorData>(NativeFeatureType.kNotificationServices);
        }

        private void SaveData()
        {
            SimulatorDatabase.Instance.SetObject(NativeFeatureType.kNotificationServices, m_simulatorData);
        }

        public static void Reset() 
        {
            SimulatorDatabase.Instance.RemoveObject(NativeFeatureType.kNotificationServices);
        }

        #endregion

        #region Permission methods

        public NotificationPermissionStatus GetPermissionStatus(out NotificationPermissionOptions enabledOptions)
        {
            enabledOptions  = m_simulatorData.EnabledOptions;

            return m_simulatorData.PermissionStatus;
        }

        public void RequestPermission(NotificationPermissionOptions options, Action<NotificationPermissionStatus, Error> callback)
        {
            // check whether required permission is already granted
            var     permissionStatus    = m_simulatorData.PermissionStatus;
            if (NotificationPermissionStatus.Authorized == permissionStatus)
            {
                callback(NotificationPermissionStatus.Authorized, new Error(description: kAlreadyAuthorizedError));
            }
            else
            {
                // show prompt to user asking for required permission
                var     newAlertDialog  = new AlertDialogBuilder()
                    .SetTitle("Notification Services Simulator")
                    .SetMessage("App would like to send you notifications.")
                    .AddButton("Authorise", () => 
                    { 
                        // save selection
                        UpdatePermissionStatus(NotificationPermissionStatus.Authorized, options);

                        // send result
                        callback(NotificationPermissionStatus.Authorized, null);
                    })
                    .AddCancelButton("Cancel", () => 
                    { 
                        // save selection
                        UpdatePermissionStatus(NotificationPermissionStatus.Denied);

                        // send result
                        callback(NotificationPermissionStatus.Denied, null);
                    })
                    .Build();
                newAlertDialog.Show();
            }
        }

        #endregion

        #region Local notification methods

        public void AddNotification(Notification notification, out Error error)
        {
            if (ValidateAccess(out error))
            {
                m_simulatorData.AddScheduledNotification(notification);

                // save state                    
                SaveData();
            }
        }

        public Notification[] GetScheduledNotifications(out Error error)
        {
            if (ValidateAccess(out error))
            {
                var     notifications   = m_simulatorData.GetScheduledNotifications();
                return notifications.ToArray();
            }

            return null;
        }

        public void RemoveScheduledNotification(Notification notification)
        {
            Error   error;
            if (ValidateAccess(out error))
            {
                m_simulatorData.RemoveNotificationWithId(notification);

                // save state                    
                SaveData();
            }
        }

        public void RemoveAllScheduledNotifications()
        {
            Error   error;
            if (ValidateAccess(out error))
            {
                m_simulatorData.RemoveAllScheduledNotifications();

                // save state                    
                SaveData();
            }
        }

        public Notification[] GetDeliveredNotifications(out Error error)
        {
            if (ValidateAccess(out error))
            {
                IEnumerable<Notification>   notifications  = m_simulatorData.GetDeliveredNotifications();
                return notifications.ToArray();
            }

            return null;
        }

        public void RemoveAllDeliveredNotifications()
        {
            Error   error;
            if (ValidateAccess(out error))
            {
                m_simulatorData.ClearDeliveredNotifications();
            }
        }

        #endregion

        #region Remote notification

        public void RegisterForRemoteNotification(Action<string, Error> callback)
        {
            // get required info
            string  deviceToken = null;
            Error   error       = null;
            if (ValidateAccess(out error))
            {
                deviceToken     = "device-token";
                m_simulatorData.IsRegistedForRemoteNotifications = true;

                // save state                    
                SaveData();
            }

            // send result
            callback(deviceToken, error);
        }

        public void UnregisterForRemoteNotification()
        {
            Error   error   = null;
            if (ValidateAccess(out error))
            {
                m_simulatorData.IsRegistedForRemoteNotifications = false;

                // save state                    
                SaveData();
            }
        }

        public bool IsRegisteredForRemoteNotification()
        {
            return m_simulatorData.IsRegistedForRemoteNotifications;
        }

        #endregion

        #region Private methods

        private bool ValidateAccess(out Error error)
        {
            // set default value
            error   = null;

            // read current status
            if (m_simulatorData.PermissionStatus == NotificationPermissionStatus.Authorized)
            {
                return true;
            }

            // set error info
            error   = new Error(description: kUnauthorizedAccessError);

            return false;
        }

        private void UpdatePermissionStatus(NotificationPermissionStatus accessStatus, NotificationPermissionOptions enabledOptions = NotificationPermissionOptions.None)
        {
            m_simulatorData.SetPermissionStatus(accessStatus, enabledOptions);

            // save state
            SaveData();
        }

        #endregion
    }
}