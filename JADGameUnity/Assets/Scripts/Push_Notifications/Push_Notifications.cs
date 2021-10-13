using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using Unity.Notifications.iOS;
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

        if(Application.platform == RuntimePlatform.Android)
        {
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
            notification.FireTime = System.DateTime.Now.AddHours(timeTillNotificationFires);

            //Send a notification.
            var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");

            //If the script is run and a message is already scheduled, cancel it and reschedule another message.
            if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
            {
                AndroidNotificationCenter.CancelAllNotifications();
                AndroidNotificationCenter.SendNotification(notification, "channel_id");
            }
        }
        else if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //IOS.

        }
       
    }

}
