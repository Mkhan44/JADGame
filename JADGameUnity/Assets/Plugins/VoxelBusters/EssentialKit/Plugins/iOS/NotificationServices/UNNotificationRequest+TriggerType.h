//
//  UNNotificationRequest+TriggerType.h
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <UserNotifications/UserNotifications.h>
#import "NPNotificationCenterDataTypes.h"

@interface UNNotificationRequest (TriggerType)

- (UNNotificationTriggerType)triggerType;

@end

