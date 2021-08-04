//
//  NPNotificationCenterDataTypes.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#include "NPDefines.h"
#include "NPKit.h"

typedef enum : NSInteger
{
    UNNotificationTriggerTypeUnknown,
    UNNotificationTriggerTypePushNotification,
    UNNotificationTriggerTypeCalendar,
    UNNotificationTriggerTypeTimeInterval,
    UNNotificationTriggerTypeLocation,
} UNNotificationTriggerType;

typedef struct
{
    UNAuthorizationStatus   authorizationStatus;
    UNNotificationSetting   alertSetting;
    UNNotificationSetting   badgeSetting;
    UNNotificationSetting   carPlaySetting;
    UNNotificationSetting   lockScreenSetting;
    UNNotificationSetting   notificationCenterSetting;
    UNNotificationSetting   soundSetting;
    UNNotificationSetting   criticalAlertSetting;
    UNNotificationSetting   announcementSetting;
    UNAlertStyle            alertStyle;
    long                    showPreviewsSetting;
} NPUnityNotificationSettings;

// callback signatures
typedef void (*RequestAuthorizationNativeCallback)(UNAuthorizationStatus status, const string error, IntPtr tagPtr);

typedef void (*GetSettingsNativeCallback)(NPUnityNotificationSettings* settingsData, IntPtr tagPtr);

typedef void (*ScheduleLocalNotificationNativeCallback)(const string error, IntPtr tagPtr);

typedef void (*GetScheduledNotificationsNativeCallback)(NPArray* arrayPtr, const string error, IntPtr tagPtr);

typedef void (*GetDeliveredNotificationsNativeCallback)(NPArray* arrayPtr, const string error, IntPtr tagPtr);

typedef void (*RegisterForRemoteNotificationsNativeCallback)(const string deviceToken, const string error, IntPtr tagPtr);

typedef void (*NotificationReceivedNativeCallback)(IntPtr nativePtr, bool isLaunchNotification);
