//
//  NPNotificationCenterBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <UserNotifications/UserNotifications.h>
#import "UNNotificationRequest+TriggerType.h"
#import "UNNotificationSettings+ManagedData.h"
#import "NPAppDelegateListener.h"
#import "NPKit.h"
#import "NPNotificationCenterDataTypes.h"

#pragma mark - Native binding methods

static RequestAuthorizationNativeCallback           _requestAuthorizationCallback;
static GetSettingsNativeCallback                    _getSettingsCallback;
static ScheduleLocalNotificationNativeCallback      _scheduleLocalNotificationCallback;
static GetScheduledNotificationsNativeCallback      _getScheduledNotificationsCallback;
static GetDeliveredNotificationsNativeCallback      _getDeliveredNotificationsCallback;
static RegisterForRemoteNotificationsNativeCallback _registerForRemoteNotificationsCallback;
static NotificationReceivedNativeCallback           _notificationReceivedCallback;

NPBINDING DONTSTRIP void NPNotificationCenterRegisterCallbacks(RequestAuthorizationNativeCallback requestAuthorizationCallback,
                                                               GetSettingsNativeCallback getSettingsCallback,
                                                               ScheduleLocalNotificationNativeCallback scheduleLocalNotificationCallback,
                                                               GetScheduledNotificationsNativeCallback getScheduledNotificationsCallback,
                                                               GetDeliveredNotificationsNativeCallback getDeliveredNotificationsCallback,
                                                               RegisterForRemoteNotificationsNativeCallback registerForRemoteNotificationCallback,
                                                               NotificationReceivedNativeCallback notificationReceivedCallback)
{
    // cache callback
    _requestAuthorizationCallback           = requestAuthorizationCallback;
    _getSettingsCallback                    = getSettingsCallback;
    _scheduleLocalNotificationCallback      = scheduleLocalNotificationCallback;
    _getScheduledNotificationsCallback      = getScheduledNotificationsCallback;
    _getDeliveredNotificationsCallback      = getDeliveredNotificationsCallback;
    _registerForRemoteNotificationsCallback = registerForRemoteNotificationCallback;
    _notificationReceivedCallback           = notificationReceivedCallback;
    
    // set remote notifiation callback responder
    __weak NPAppDelegateListener*   delegateListener    = [NPAppDelegateListener sharedListener];
    [delegateListener setNotificationRecievedCompletionHandler:^(UNNotification *notification) {
        // send callback
        void*   notificationRequestPtr  = (__bridge IntPtr)notification.request;
        _notificationReceivedCallback(notificationRequestPtr, false);
    }];
}

NPBINDING DONTSTRIP void NPNotificationCenterInit(UNNotificationPresentationOptions presentationOptions)
{
    [[NPAppDelegateListener sharedListener] setPresentationOptions:presentationOptions];
}

NPBINDING DONTSTRIP void NPNotificationCenterRequestAuthorization(UNAuthorizationOptions options, IntPtr tagPtr)
{
    [[UNUserNotificationCenter currentNotificationCenter] requestAuthorizationWithOptions:options
                                                                        completionHandler:^(BOOL granted, NSError * _Nullable error) {
        // get settings
        [[UNUserNotificationCenter currentNotificationCenter] getNotificationSettingsWithCompletionHandler:^(UNNotificationSettings* _Nonnull settings) {
            // send callback
            _requestAuthorizationCallback(settings.authorizationStatus, NPCreateCStringFromNSError(error), tagPtr);
        }];
    }];
}

NPBINDING DONTSTRIP void NPNotificationCenterGetSettings(IntPtr tagPtr)
{
    [[UNUserNotificationCenter currentNotificationCenter] getNotificationSettingsWithCompletionHandler:^(UNNotificationSettings* _Nonnull settings) {
        // send callback
        NPUnityNotificationSettings settingsData    = [settings toManagedData];
        _getSettingsCallback(&settingsData, tagPtr);
    }];
}

NPBINDING DONTSTRIP void NPNotificationCenterScheduleLocalNotification(IntPtr notificationRequestPtr, IntPtr tagPtr)
{
    UNNotificationRequest*     notificationRequest      = (__bridge UNNotificationRequest*)notificationRequestPtr;
    [[UNUserNotificationCenter currentNotificationCenter] addNotificationRequest:notificationRequest withCompletionHandler:^(NSError* _Nullable error) {
        // send event
        _scheduleLocalNotificationCallback(NPCreateCStringFromNSError(error), tagPtr);
    }];
}

NPBINDING DONTSTRIP void NPNotificationCenterGetScheduledNotifications(IntPtr tagPtr)
{
    [[UNUserNotificationCenter currentNotificationCenter] getPendingNotificationRequestsWithCompletionHandler:^(NSArray<UNNotificationRequest*>* _Nonnull requests) {
        // create c array
        NPArray*    cArray  = NPCreateNativeArrayFromNSArray(requests);
        
        // send data
        _getScheduledNotificationsCallback(cArray, nil, tagPtr);
        
        // release c properties
        delete(cArray);
    }];
}

NPBINDING DONTSTRIP void NPNotificationCenterRemovePendingNotification(const char* notificationId)
{
    [[UNUserNotificationCenter currentNotificationCenter] removePendingNotificationRequestsWithIdentifiers:@[NPCreateNSStringFromCString(notificationId)]];
}

NPBINDING DONTSTRIP void NPNotificationCenterRemoveAllPendingNotifications()
{
    [[UNUserNotificationCenter currentNotificationCenter] removeAllPendingNotificationRequests];
}

NPBINDING DONTSTRIP void NPNotificationCenterRemoveAllDeliveredNotifications()
{
    [[UNUserNotificationCenter currentNotificationCenter] removeAllDeliveredNotifications];
}

NPBINDING DONTSTRIP void NPNotificationCenterGetDeliveredNotifications(IntPtr tagPtr)
{
    [[UNUserNotificationCenter currentNotificationCenter] getDeliveredNotificationsWithCompletionHandler:^(NSArray<UNNotification*>* _Nonnull notifications) {
        // create array of notification requests
        NSMutableArray*    notificationRequests    = [NSMutableArray arrayWithCapacity:[notifications count]];
        for (int iter = 0; iter < notificationRequests.count; iter++)
        {
            [notificationRequests addObject:[notifications[iter] request]];
        }
        
        // create c array
        NPArray*    cArray  = NPCreateNativeArrayFromNSArray(notificationRequests);
        
        // send data
        _getDeliveredNotificationsCallback(cArray, nil, tagPtr);
        
        // release c properties
        delete(cArray);
    }];
}

NPBINDING DONTSTRIP void NPNotificationCenterRegisterForRemoteNotifications(IntPtr tagPtr)
{
    __weak NPAppDelegateListener*  delegateListener    = [NPAppDelegateListener sharedListener];
    [delegateListener setRegisterForRemoteNotificationsCompletionHandler:^(NSString* deviceToken, NSError* error) {
        // send callback
        _registerForRemoteNotificationsCallback(NPCreateCStringFromNSString(deviceToken), NPCreateCStringFromNSError(error), tagPtr);
        
        // reset properties
        [delegateListener setRegisterForRemoteNotificationsCompletionHandler:nil];
    }];
    
    [[UIApplication sharedApplication] registerForRemoteNotifications];
}

NPBINDING DONTSTRIP bool NPNotificationCenterIsRegisteredForRemoteNotifications()
{
    return [[UIApplication sharedApplication] isRegisteredForRemoteNotifications];
}

NPBINDING DONTSTRIP void NPNotificationCenterUnregisterForRemoteNotifications()
{
    [[UIApplication sharedApplication] unregisterForRemoteNotifications];
}

NPBINDING DONTSTRIP void NPNotificationCenterSetApplicationIconBadgeNumber(int count)
{
    [UIApplication sharedApplication].applicationIconBadgeNumber = count;
}
