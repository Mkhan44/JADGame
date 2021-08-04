using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
    public class NotificationServicesDemo : DemoActionPanelBase<NotificationServicesDemoAction, NotificationServicesDemoActionType>
    {
        #region Fields

        [SerializeField]
        private     RectTransform[]     m_accessDependentObjects                = null;

        [SerializeField]
        private     InputField          m_idInputField                          = null;

        [SerializeField]
        private     InputField          m_titleInputField                       = null;

        [SerializeField]
        private     InputField          m_timeIntervalInputField                = null;

        [SerializeField]
        private     InputField          m_cancelNotificationIdInputField        = null;

        #endregion

        #region Base class methods

        protected override void Start()
        {
            base.Start();

            // set object state
            NotificationServices.GetSettings((result) => OnPermissionStatusChange(result.Settings.PermissionStatus));
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // register for events
            NotificationServices.OnNotificationReceived += OnNotificationReceived;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            // register for events
            NotificationServices.OnNotificationReceived -= OnNotificationReceived;
        }

        protected override void OnActionSelectInternal(NotificationServicesDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case NotificationServicesDemoActionType.RequestAccess:
                    NotificationServices.RequestPermission(NotificationPermissionOptions.Alert | NotificationPermissionOptions.Sound | NotificationPermissionOptions.Badge | NotificationPermissionOptions.ProvidesAppNotificationSettings, callback: (result, error) =>
                    {
                        Log("Request for access finished.");
                        Log("Notification access status: " + result.PermissionStatus);

                        // update ui 
                        OnPermissionStatusChange(result.PermissionStatus);
                    });
                    break;

                case NotificationServicesDemoActionType.GetSettings:
                    NotificationServices.GetSettings((result) =>
                    {
                        var settings = result.Settings;
                        // update console
                        Log(settings.ToString());

                        // update ui 
                        OnPermissionStatusChange(settings.PermissionStatus);
                    });
                    break;

                case NotificationServicesDemoActionType.ScheduleNotificationNow:
                    string newNotificationID1 = m_idInputField.text;
                    string newNotificationTitle1 = m_titleInputField.text;
                    if (string.IsNullOrEmpty(newNotificationID1))
                    {
                        Log("Provide notification id.");
                        return;
                    }
                    if (string.IsNullOrEmpty(newNotificationTitle1))
                    {
                        Log("Provide notification title.");
                        return;
                    }
                    // create notification
                    var newNotification1 = NotificationBuilder.CreateNotification(newNotificationID1)
                        .SetTitle(newNotificationTitle1)
                        .SetUserInfo(new System.Collections.Generic.Dictionary<string, string> { { "Test", "Value"} })
                        .SetBadge(10)
                        .Create();
                    // schedule
                    NotificationServices.ScheduleNotification(newNotification1, (error) =>
                    {
                        if (error == null)
                        {
                            Log("Request to schedule notification finished successfully.");
                        }
                        else
                        {
                            Log("Request to schedule notification failed with error. Error: " + error);
                        }
                    });
                    break;

                case NotificationServicesDemoActionType.ScheduleNotification:
                    string  newNotificationID2      = m_idInputField.text;
                    string  newNotificationTitle2   = m_titleInputField.text;
                    long    timeInterval;
                    long.TryParse(m_timeIntervalInputField.text, out timeInterval);
                    if (string.IsNullOrEmpty(newNotificationID2))
                    {
                        Log("Provide notification id.");
                        return;
                    }
                    if (string.IsNullOrEmpty(newNotificationTitle2))
                    {
                        Log("Provide notification title.");
                        return;
                    }
                    if (timeInterval < 1)
                    {
                        Log("Provide a valid time interval value.");
                        return;
                    }
                    // create notification
                    var     newNotification2        = NotificationBuilder.CreateNotification(newNotificationID2)
                        .SetTitle(newNotificationTitle2)
                        .SetTimeIntervalNotificationTrigger(timeInterval)
                        .Create();
                    // schedule notification
                    NotificationServices.ScheduleNotification(newNotification2, (error) =>
                    {
                        if (error == null)
                        {
                            Log("Request to schedule notification completed successfully.");
                        }
                        else
                        {
                            Log("Request to schedule notification failed with error. Error: " + error);
                        }
                    });
                    break;

                case NotificationServicesDemoActionType.GetScheduledNotifications:
                    NotificationServices.GetScheduledNotifications((result, error) =>
                    {
                        if (error == null)
                        {
                            // show console messages
                            var     notifications   = result.Notifications;
                            Log("Request for fetch scheduled notifications finished successfully.");
                            Log("Total notifications scheduled: " + notifications.Length);
                            Log("Below are the notifications:");
                            for (int iter = 0; iter < notifications.Length; iter++)
                            {
                                var     notification    = notifications[iter];
                                Log(string.Format("[{0}]: {1}", iter, notification));
                                Debug.Log("User info : " + notification.UserInfo);
                            }
                        }
                        else
                        {
                            Log("Request for fetch scheduled notifications failed with error. Error: " + error);
                        }
                    });
                    break;

                case NotificationServicesDemoActionType.CancelScheduledNotification:
                    string  cancelNotificationID    = m_cancelNotificationIdInputField.text;
                    if (string.IsNullOrEmpty(cancelNotificationID))
                    {
                        Log("Provide notification id.");
                        return;
                    }
                    NotificationServices.CancelScheduledNotification(cancelNotificationID);
                    Log("Cancelling notification with id: " + cancelNotificationID);
                    break;

                case NotificationServicesDemoActionType.CancelAllScheduledNotifications:
                    NotificationServices.CancelAllScheduledNotifications();
                    Log("Cancelling all the notifications.");
                    break;

                case NotificationServicesDemoActionType.GetDeliveredNotifications:
                    NotificationServices.GetDeliveredNotifications((result, error) =>
                    {
                        if (error == null)
                        {
                            // show console messages
                            var     notifications   = result.Notifications;
                            Log("Request for fetch delivered notifications finished successfully.");
                            Log("Total notifications received: " + notifications.Length);
                            Log("Below are the notifications:");
                            for (int iter = 0; iter < notifications.Length; iter++)
                            {
                                var     notification    = notifications[iter];
                                Log(string.Format("[{0}]: {1}", iter, notification));
                            }
                        }
                        else
                        {
                            Log("Request for fetch delivered notifications failed with error. Error: " + error);
                        }
                    });
                    break;

                case NotificationServicesDemoActionType.RemoveAllDeliveredNotifications:
                    NotificationServices.RemoveAllDeliveredNotifications();
                    Log("Removing all the delivered notifications.");
                    break;

                case NotificationServicesDemoActionType.DeviceToken:
                    string  deviceToken1    = NotificationServices.CachedSettings.DeviceToken;
                    Log("Device token: " + deviceToken1);
                    break;

                case NotificationServicesDemoActionType.RegisterForRemoteNotifications:
                    NotificationServices.RegisterForPushNotifications((result, error) =>
                    {
                        if (error == null)
                        {
                            Log("Remote notification registration finished successfully. Device token: " + result.DeviceToken);
                        }
                        else
                        {
                            Log("Remote notification registration failed with error. Error: " + error.Description);
                        }
                    });
                    break;

                case NotificationServicesDemoActionType.IsRegisteredForRemoteNotifications:
                    bool    isRegistered    = NotificationServices.IsRegisteredForPushNotifications();
                    Log("Is registered for remote notifications: " + isRegistered);
                    break;

                case NotificationServicesDemoActionType.UnregisterForRemoteNotifications:
                    NotificationServices.UnregisterForPushNotifications();
                    Log("Unregistering from receiving remote notifications.");
                    break;

                case NotificationServicesDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kNotificationServices);
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Private methods
        
        private void OnPermissionStatusChange(NotificationPermissionStatus newStatus)
        {
            // update UI
            bool    active  = (newStatus == NotificationPermissionStatus.Authorized);
            foreach (var rect in m_accessDependentObjects)
            {
                rect.gameObject.SetActive(active);
            }
        }

        private void OnNotificationReceived(NotificationServicesNotificationReceivedResult data)
        {
            Log(string.Format("{0} received.", data.Notification));
        }

        #endregion
    }
}
