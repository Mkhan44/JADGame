#if UNITY_IOS || UNITY_TVOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{
    internal static class NotificationBinding
    {
        [DllImport("__Internal")]
        public static extern IntPtr NPNotificationRequestCreate(string id, IntPtr contentPtr, IntPtr triggerPtr);

        [DllImport("__Internal")]
        public static extern string NPNotificationRequestGetId(IntPtr requestPtr);

        [DllImport("__Internal")]
        public static extern UNNotificationTriggerType NPNotificationRequestGetTriggerType(IntPtr requestPtr);

        [DllImport("__Internal")]
        public static extern IntPtr NPNotificationRequestGetContent(IntPtr requestPtr);

        [DllImport("__Internal")]
        public static extern IntPtr NPNotificationRequestGetTrigger(IntPtr requestPtr);

        [DllImport("__Internal")]
        public static extern IntPtr NPNotificationContentCreate();

        [DllImport("__Internal")]
        public static extern string NPNotificationContentGetTitle(IntPtr contentPtr);

        [DllImport("__Internal")]
        public static extern void NPNotificationContentSetTitle(IntPtr contentPtr, string value);

        [DllImport("__Internal")]
        public static extern string NPNotificationContentGetSubtitle(IntPtr contentPtr);

        [DllImport("__Internal")]
        public static extern void NPNotificationContentSetSubtitle(IntPtr contentPtr, string value);

        [DllImport("__Internal")]
        public static extern string NPNotificationContentGetBody(IntPtr contentPtr);

        [DllImport("__Internal")]
        public static extern void NPNotificationContentSetBody(IntPtr contentPtr, string value);

        [DllImport("__Internal")]
        public static extern int NPNotificationContentGetBadge(IntPtr contentPtr);

        [DllImport("__Internal")]
        public static extern void NPNotificationContentSetBadge(IntPtr contentPtr, int value);

        [DllImport("__Internal")]
        public static extern string NPNotificationContentGetUserInfo(IntPtr contentPtr);

        [DllImport("__Internal")]
        public static extern void NPNotificationContentSetUserInfo(IntPtr contentPtr, string jsonStr);

        [DllImport("__Internal")]
        public static extern void NPNotificationContentSetSoundName(IntPtr contentPtr, string soundName);

        [DllImport("__Internal")]
        public static extern string NPNotificationContentGetLaunchImageName(IntPtr contentPtr);

        [DllImport("__Internal")]
        public static extern void NPNotificationContentSetLaunchImageName(IntPtr contentPtr, string value);

        [DllImport("__Internal")]
        public static extern string NPNotificationContentGetCategoryId(IntPtr contentPtr);

        [DllImport("__Internal")]
        public static extern bool NPNotificationTriggerGetRepeats(IntPtr triggerPtr);

        [DllImport("__Internal")]
        public static extern IntPtr NPTimeIntervalNotificationTriggerCreate(double interval, bool repeats);

        [DllImport("__Internal")]
        public static extern void NPTimeIntervalNotificationTriggerGetProperties(IntPtr triggerPtr, ref double timeInterval, ref string nextTriggerDate, ref bool repeats);

        [DllImport("__Internal")]
        public static extern IntPtr NPCalendarNotificationTriggerCreate(UnityDateComponents dateComponents, bool repeats);

        [DllImport("__Internal")]
        public static extern string NPCalendarNotificationTriggerGetProperties(IntPtr triggerPtr, ref UnityDateComponents dateComponents, ref string nextTriggerDate, ref bool repeats);

        [DllImport("__Internal")]
        public static extern IntPtr NPLocationNotificationTriggerCreate(UnityCircularRegion regionData, bool notifyOnEntry, bool notifyOnExit, bool repeats);

        [DllImport("__Internal")]
        public static extern void NPLocationNotificationTriggerGetProperties(IntPtr triggerPtr, ref UnityCircularRegion regionData, ref bool notifyOnEntry, ref bool notifyOnExit, ref bool repeats);
    }
}
#endif