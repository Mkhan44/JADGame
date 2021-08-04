//
//  NPAppDelegateListener.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <UserNotifications/UserNotifications.h>
#import "AppDelegateListener.h"
#import "NPNotificationCenterDataTypes.h"

typedef void (^RegisterForRemoteNotificationsCompletionHandler)(NSString* deviceToken, NSError* error);
typedef void (^NotificationRecievedCompletionHandler)(UNNotification* notification);

@interface NPAppDelegateListener : NSObject<AppDelegateListener
                                #if NATIVE_PLUGINS_USES_NOTIFICATION
                                    , UNUserNotificationCenterDelegate
                                #endif
                                    >

// properties
@property(nonatomic, copy) RegisterForRemoteNotificationsCompletionHandler      registerForRemoteNotificationsCompletionHandler;
@property(nonatomic, copy) NotificationRecievedCompletionHandler                notificationRecievedCompletionHandler;
@property(nonatomic) UNNotificationPresentationOptions                          presentationOptions;

// static methods
+ (NPAppDelegateListener*)sharedListener;

@end
