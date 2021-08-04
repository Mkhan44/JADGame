#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.NativePlugins.iOS;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{
    internal static class NotificationTrigger
    {
        #region Create methods

        public static INotificationTrigger CreateNotificationTrigger(IntPtr requestPtr, IntPtr triggerPtr)
        {
            // get trigger type
            var     triggerType     = NotificationBinding.NPNotificationRequestGetTriggerType(requestPtr);
            switch (triggerType)
            {
                case UNNotificationTriggerType.UNNotificationTriggerTypeTimeInterval:
                    return CreateTimeIntervalNotificationTrigger(triggerPtr);

                case UNNotificationTriggerType.UNNotificationTriggerTypeCalendar:
                    return CreateCalendarNotificationTrigger(triggerPtr);

                case UNNotificationTriggerType.UNNotificationTriggerTypeLocation:
                    return CreateLocationNotificationTrigger(triggerPtr);

                case UNNotificationTriggerType.UNNotificationTriggerTypePushNotification:
                    return CreatePushNotificationTrigger();

                default:
                    throw VBException.SwitchCaseNotImplemented(triggerType);
            }
        }

        public static TimeIntervalNotificationTrigger CreateTimeIntervalNotificationTrigger(IntPtr triggerPtr)
        {
            // get properties
            double  timeInterval        = 0;
            string  nextTriggerStr      = null;
            bool    repeats             = false;
            NotificationBinding.NPTimeIntervalNotificationTriggerGetProperties(triggerPtr, ref timeInterval, ref nextTriggerStr,  ref repeats);
            var     nextTriggerDate     = IosNativePluginsUtility.ParseDateTimeStringInUTCFormat(nextTriggerStr);

            // create object
            return new TimeIntervalNotificationTrigger(timeInterval, repeats, nextTriggerDate.ToLocalTime());
        }

        public static CalendarNotificationTrigger CreateCalendarNotificationTrigger(IntPtr triggerPtr)
        {
            // get properties
            var     dateComponents      = new UnityDateComponents();
            string  nextTriggerStr      = null;
            bool    repeats             = false;
            NotificationBinding.NPCalendarNotificationTriggerGetProperties(triggerPtr, ref dateComponents, ref nextTriggerStr, ref repeats);
            var     nextTriggerDate     = IosNativePluginsUtility.ParseDateTimeStringInUTCFormat(nextTriggerStr);

            // create object
            return new CalendarNotificationTrigger(dateComponents, repeats, nextTriggerDate.ToLocalTime());
        }

        public static LocationNotificationTrigger CreateLocationNotificationTrigger(IntPtr triggerPtr)
        {
            // get properties
            var     region          = new UnityCircularRegion();
            bool    notifyOnEntry   = false;
            bool    notifyOnExit    = false;
            bool    repeats         = false;
            NotificationBinding.NPLocationNotificationTriggerGetProperties(triggerPtr, ref region, ref notifyOnEntry, ref notifyOnExit, ref repeats);

            // create object
            return new LocationNotificationTrigger(region, notifyOnEntry, notifyOnExit, repeats);
        }

        public static PushNotificationTrigger CreatePushNotificationTrigger()
        {
            return new PushNotificationTrigger();
        }

        #endregion
    }
}
#endif