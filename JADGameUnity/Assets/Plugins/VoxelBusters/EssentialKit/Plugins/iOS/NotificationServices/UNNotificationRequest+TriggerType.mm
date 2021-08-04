//
//  UNNotificationRequest+TriggerType.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import "UNNotificationRequest+TriggerType.h"

@implementation UNNotificationRequest (TriggerType)

- (UNNotificationTriggerType)triggerType
{
    __weak UNNotificationTrigger*   trigger = [self trigger];
    if ([trigger isKindOfClass:[UNPushNotificationTrigger class]])
    {
        return UNNotificationTriggerTypePushNotification;
    }
    else if ([trigger isKindOfClass:[UNLocationNotificationTrigger class]])
    {
        return UNNotificationTriggerTypeLocation;
    }
    else if ([trigger isKindOfClass:[UNCalendarNotificationTrigger class]])
    {
        return UNNotificationTriggerTypeCalendar;
    }
    else if ([trigger isKindOfClass:[UNTimeIntervalNotificationTrigger class]])
    {
        return UNNotificationTriggerTypeTimeInterval;
    }
    
    return UNNotificationTriggerTypeUnknown;
}

@end
