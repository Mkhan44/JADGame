//Code written by Mohamed Riaz Khan of Bukugames.
//All code is written by me (Above name) unless otherwise stated via comments below.

//*Note* Documentation from the Unity documentation website was used to assist in writing this code.
//YouTube tutorials from the official Unity YouTube channel also used to assist in writing this code.
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif
using UnityEngine;


public class Push_Notifications : MonoBehaviour
{
    double timeTillNotificationFires;
    [SerializeField] DayChecker dailyRewardChecker;
    // Start is called before the first frame update
    void Start()
    {
        //Seeing how long to wait until we give the player a notification to claim their rewards.
        TimeSpan theDiff = dailyRewardChecker.getDifference();
        if (theDiff.TotalDays > 0 && theDiff.TotalDays < 1)
        {
            //Find the TotalDays in decimal form. Multiply that by 24 for a full day.
            //From there, that should be the hours that have passed since you last claimed.
            //Subtract that total from 24 to get the hours UNTIL you can claim.
            //EX: Claimed 6 hours ago. The TotalDays will return 0.25. 0.25*24 = 6. 24-6 = 18. So 18 hours till we can claim.
            double hoursLeftTillClaim = theDiff.TotalDays * 24;
            hoursLeftTillClaim = 24 - hoursLeftTillClaim;
            timeTillNotificationFires = Math.Round(hoursLeftTillClaim,2);
        }
        else if (theDiff.TotalDays < 0)
        {
            Debug.LogWarning("Hey something is wrong...This should not be less than 0 for claiming daily rewards.");
            timeTillNotificationFires = 24f;
        }
        //Over 1 full day since claim, just give 'em an hour until we spam 'em with notifs.
        else
        {
            timeTillNotificationFires = 1f;
        }
        Debug.Log("Time till we can claim is: " + timeTillNotificationFires);

#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            //Android

            //Remove notifications that have already been displayed.
            AndroidNotificationCenter.CancelAllDisplayedNotifications();

            //Create the Android Notification Channel to send messages through.
            var channel = new AndroidNotificationChannel()
            {
                Id = "channel_id",
                Name = "Notifications Channel",
                Importance = Importance.Default,
                Description = "Reminder notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            var notification = new AndroidNotification();
            notification.Title = "Your daily reward is ready!";
            notification.Text = "Collect your free coins now!";
            notification.LargeIcon = "rprt_large_icon";
            notification.SmallIcon = "rprt_small_icon";
             notification.FireTime = System.DateTime.Now.AddHours(timeTillNotificationFires);
            //notification.FireTime = System.DateTime.Now.AddSeconds(15);

            //Send a notification.
            var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

            //If the script is run and a message is already scheduled, cancel it and reschedule another message.
            if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
            {
                AndroidNotificationCenter.CancelAllNotifications();
                AndroidNotificationCenter.SendNotification(notification, "channel_id");
            }

            return;
        }
#endif

#if UNITY_IOS

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //IOS.

            //Removes notifications that have already been delivered.
            iOSNotificationCenter.RemoveAllDeliveredNotifications();

            //Request the authorization to send a notification.

            //If not determined, request authorization with popup.
            if (iOSNotificationCenter.GetNotificationSettings().AuthorizationStatus == AuthorizationStatus.NotDetermined)
            {
                StartCoroutine(RequestAuthorization());
            }
            //Don't send notifs since they denied access.
            else if(iOSNotificationCenter.GetNotificationSettings().AuthorizationStatus == AuthorizationStatus.Denied)
            {
                if (iOSNotificationCenter.GetScheduledNotifications() != null)
                {
                    iOSNotificationCenter.RemoveAllScheduledNotifications();
                }
                return;
            }

            int hoursToWait = (int)Math.Ceiling(timeTillNotificationFires);
            Debug.LogWarning("HOURS TO WAIT ARE: " + hoursToWait);
            //Time between notifications.
            var timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                //Do some math to figure out hours & minutes & seconds and plug those in from the timeTillNotificationFires variable.

                TimeInterval = new TimeSpan(hoursToWait, 0, 0),
                Repeats = false
            };

            //Setup the content of the notification.
            iOSNotification iosNotif = new iOSNotification()
            {
                Identifier = "RPRT_Daily_Reward_Notification",
                Title = "RPRT Daily Reward Notification",
                Subtitle = "Collect your daily reward!",
                Body = "Collect your free coins now!",
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };

            //Send the notification after X amount of time has passed, X being the timeTrigger.
            iOSNotificationCenter.ScheduleNotification(iosNotif);

            //Send the notification after X amount of time has passed, X being the timeTrigger.
            if (iOSNotificationCenter.GetScheduledNotifications() != null)
            {
                iOSNotificationCenter.RemoveAllScheduledNotifications();
                iOSNotificationCenter.ScheduleNotification(iosNotif);
            }
            return;
           
        }
       
    }

    IEnumerator RequestAuthorization()
    {
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            while (!req.IsFinished)
            {
                yield return null;
            };

            string res = "\n RequestAuthorization:";
            res += "\n finished: " + req.IsFinished;
            res += "\n granted :  " + req.Granted;
            res += "\n error:  " + req.Error;
            res += "\n deviceToken:  " + req.DeviceToken;
            Debug.Log(res);
        }
#endif
    }
}
