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
    internal sealed class MutableNotification : Notification, IMutableNotification
    {
        #region Constructors

        public MutableNotification(string notificationId)
            : base(notificationId)
        {
            // set properties
            var     contentPtr      = NotificationBinding.NPNotificationContentCreate();
            SetNativeContentInternal(new IosNativeObjectRef(contentPtr, retain: false));
        }

        ~MutableNotification()
        {
            Dispose(false);
        }

        #endregion

        #region INotificationPropertySetter implementation

        public void SetTitle(string value)
        {
            NotificationBinding.NPNotificationContentSetTitle(GetNativeContentInternal().Pointer, value);
        }

        public void SetSubtitle(string value)
        {
            NotificationBinding.NPNotificationContentSetSubtitle(GetNativeContentInternal().Pointer, value);
        }

        public void SetBody(string value)
        {
            NotificationBinding.NPNotificationContentSetBody(GetNativeContentInternal().Pointer, value);
        }

        public void SetBadge(int value)
        {
            NotificationBinding.NPNotificationContentSetBadge(GetNativeContentInternal().Pointer, value);
        }

        public void SetUserInfo(IDictionary value)
        {
            string  jsonStr     = ExternalServiceProvider.JsonServiceProvider.ToJson(value);
            NotificationBinding.NPNotificationContentSetUserInfo(GetNativeContentInternal().Pointer, jsonStr);
        }
        
        public void SetSoundFileName(string value)
        {
            NotificationBinding.NPNotificationContentSetSoundName(GetNativeContentInternal().Pointer, value);
        }
        
        public void SetIosProperties(NotificationIosProperties value)
        { 
            // copy new value
            m_iosProperties     = value;

            // sync property values
            if (!string.IsNullOrEmpty(m_iosProperties.LaunchImageFileName))
            {
                NotificationBinding.NPNotificationContentSetLaunchImageName(GetNativeContentInternal().Pointer, value.LaunchImageFileName);
            }
        }
        
        public void SetAndroidProperties(NotificationAndroidProperties value)
        { }

        public void SetTrigger(INotificationTrigger trigger)
        {
            if (!CanCreateTrigger())
            {
                return;
            }

            // set property
            var     triggerPtr  = CreateNativeTrigger(trigger);
            SetTriggerInternal(trigger, new IosNativeObjectRef(triggerPtr, retain: false));
        }

        #endregion

        #region Private methods
        
        private IntPtr CreateNativeTrigger(INotificationTrigger trigger)
        {
            if (trigger is TimeIntervalNotificationTrigger)
            {
                TimeIntervalNotificationTrigger     timeIntervalTrigger = (TimeIntervalNotificationTrigger)trigger;
                return NotificationBinding.NPTimeIntervalNotificationTriggerCreate(timeIntervalTrigger.TimeInterval, timeIntervalTrigger.Repeats);
            }

            if (trigger is CalendarNotificationTrigger)
            {
                CalendarNotificationTrigger         calendarTrigger     = (CalendarNotificationTrigger)trigger;
                return NotificationBinding.NPCalendarNotificationTriggerCreate(calendarTrigger.DateComponent, calendarTrigger.Repeats);
            }

            if (trigger is LocationNotificationTrigger)
            {
                LocationNotificationTrigger         locationTrigger     = (LocationNotificationTrigger)trigger;
                UnityCircularRegion                 region              = locationTrigger.Region;
                return NotificationBinding.NPLocationNotificationTriggerCreate(region, locationTrigger.NotifyOnEntry, locationTrigger.NotifyOnExit, locationTrigger.Repeats);
            }

            throw VBException.NotSupported(trigger.ToString());
        }

        #endregion

        #region Internal methods

        internal void SetRequestPtr(IntPtr requestPtr, bool retain)
        {
            NativeObjectRef     = new IosNativeObjectRef(requestPtr, retain);
        }

        #endregion
    }
}
#endif