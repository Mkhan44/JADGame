//
//  NPAppDelegateListener.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "NPAppDelegateListener.h"
#import "NPKit.h"
#import "UNNotificationRequest+TriggerType.h"

#define kDefaultPresentationOption  (UNNotificationPresentationOptionBadge | UNNotificationPresentationOptionSound | UNNotificationPresentationOptionAlert)

static NPAppDelegateListener*   _sharedListener;
#if NATIVE_PLUGINS_USES_NOTIFICATION
static UNNotification*          _deliveredNotification;
#endif

@implementation NPAppDelegateListener

@synthesize registerForRemoteNotificationsCompletionHandler = _registerForRemoteNotificationsCompletionHandler;
@synthesize notificationRecievedCompletionHandler           = _notificationRecievedCompletionHandler;
@synthesize presentationOptions                             = _presentationOptions;

+ (NPAppDelegateListener*)sharedListener
{
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _sharedListener  = [[NPAppDelegateListener alloc] init];
    });
    
    return _sharedListener;
}

#if NATIVE_PLUGINS_USES_NOTIFICATION
+ (void)load
{
    UnityRegisterAppDelegateListener([NPAppDelegateListener sharedListener]);
}
#endif

#pragma mark - Init methods

- (id)init
{
    self    = [super init];
    if (self)
    {
#if NATIVE_PLUGINS_USES_NOTIFICATION
        self.presentationOptions    = kDefaultPresentationOption;
        [[UNUserNotificationCenter currentNotificationCenter] setDelegate:self];
#endif
    }
    return self;
}

- (void)dealloc
{
#if NATIVE_PLUGINS_USES_NOTIFICATION
    [[UNUserNotificationCenter currentNotificationCenter] setDelegate:nil];
#endif
}

- (void)setNotificationRecievedCompletionHandler:(NotificationRecievedCompletionHandler)notificationRecievedCompletionHandler
{
#if NATIVE_PLUGINS_USES_NOTIFICATION
    _notificationRecievedCompletionHandler       = notificationRecievedCompletionHandler;
    
    // send cached info
    if (_deliveredNotification)
    {
        _notificationRecievedCompletionHandler(_deliveredNotification);
        _deliveredNotification          = nil;
    }
#endif
}

#pragma mark - AppDelegateListener methods

#if NATIVE_PLUGINS_USES_PUSH_NOTIFICATION
- (void)didRegisterForRemoteNotificationsWithDeviceToken:(NSNotification*)notification
{
    NSData*     deviceToken         = (NSData*)notification.userInfo;
    NSString*   deviceTokenStr      = NPExtractTokenFromNSData(deviceToken);
    
    // send event
    _registerForRemoteNotificationsCompletionHandler(deviceTokenStr, nil);
}

- (void)didFailToRegisterForRemoteNotificationsWithError:(NSNotification*)notification
{
    NSError*     error              = (NSError*)notification.userInfo;
    
    // send event
    _registerForRemoteNotificationsCompletionHandler(nil, error);
}
#endif

#pragma mark - UNUserNotificationCenterDelegate methods

#if NATIVE_PLUGINS_USES_NOTIFICATION
- (void)userNotificationCenter:(UNUserNotificationCenter*)center willPresentNotification:(UNNotification*)notification withCompletionHandler:(void (^)(UNNotificationPresentationOptions options))completionHandler
{
    [self handleReceivedNotification:notification];
    
    completionHandler(self.presentationOptions);
}

- (void)userNotificationCenter:(UNUserNotificationCenter *)center didReceiveNotificationResponse:(UNNotificationResponse *)response withCompletionHandler:(void(^)(void))completionHandler
{
    [self handleReceivedNotification:response.notification];
    
    completionHandler();
}

- (void)handleReceivedNotification:(UNNotification*)notification
{
#if !NATIVE_PLUGINS_USES_PUSH_NOTIFICATION
    UNNotificationTriggerType   triggerType = [[notification request] triggerType];
    if (triggerType == UNNotificationTriggerTypePushNotification)
    {
        return;
    }
#endif

    // save info if notification is received before initialisation
    if (_notificationRecievedCompletionHandler == nil)
    {
        _deliveredNotification  = notification;
    }
    else
    {
        _notificationRecievedCompletionHandler(notification);
    }
}
#endif

@end
