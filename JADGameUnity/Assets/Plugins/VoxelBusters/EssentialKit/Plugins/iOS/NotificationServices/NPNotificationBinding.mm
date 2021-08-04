//
//  NPNotificationBinding.mm
//  Native Plugins
//
//  Created by Ashwin kumar on 22/01/19.
//  Copyright (c) 2019 Voxel Busters Interactive LLP. All rights reserved.
//

#import <UserNotifications/UserNotifications.h>
#import <CoreLocation/CoreLocation.h>
#import "NPNotificationCenterDataTypes.h"
#import "UNNotificationRequest+TriggerType.h"
#import "NPKit.h"

NPBINDING DONTSTRIP IntPtr NPNotificationRequestCreate(const char* id, IntPtr contentPtr, IntPtr triggerPtr)
{
    // create notification request
    UNNotificationContent*     content                  = (__bridge UNNotificationContent*)contentPtr;
    UNNotificationTrigger*     trigger                  = (triggerPtr == nil) ? nil : (__bridge UNNotificationTrigger*)triggerPtr;
    UNNotificationRequest*     notificationRequest      = [UNNotificationRequest requestWithIdentifier:NPCreateNSStringFromCString(id)
                                                                                               content:content
                                                                                               trigger:trigger];
    return NPRetainWithOwnershipTransfer(notificationRequest);
}

NPBINDING DONTSTRIP const char* NPNotificationRequestGetId(IntPtr requestPtr)
{
    UNNotificationRequest*              request     = (__bridge UNNotificationRequest*)requestPtr;
    return NPCreateCStringCopyFromNSString(request.identifier);
}

NPBINDING DONTSTRIP UNNotificationTriggerType NPNotificationRequestGetTriggerType(IntPtr requestPtr)
{
    UNNotificationRequest*              request     = (__bridge UNNotificationRequest*)requestPtr;
    return [request triggerType];
}

NPBINDING DONTSTRIP IntPtr NPNotificationRequestGetContent(IntPtr requestPtr)
{
    UNNotificationRequest*              request     = (__bridge UNNotificationRequest*)requestPtr;
    return NPRetainWithOwnershipTransfer(request.content);
}

NPBINDING DONTSTRIP IntPtr NPNotificationRequestGetTrigger(IntPtr requestPtr)
{
    UNNotificationRequest*              request     = (__bridge UNNotificationRequest*)requestPtr;
    UNNotificationTrigger*              trigger     = request.trigger;
    return trigger ? NPRetainWithOwnershipTransfer(trigger) : nil;
}

NPBINDING DONTSTRIP IntPtr NPNotificationContentCreate()
{
    UNMutableNotificationContent*       content     = [[UNMutableNotificationContent alloc] init];
    return NPRetainWithOwnershipTransfer(content);
}

NPBINDING DONTSTRIP const char* NPNotificationContentGetTitle(IntPtr contentPtr)
{
#if !TARGET_OS_TV
    UNNotificationContent*              content     = (__bridge UNNotificationContent*)contentPtr;
    return NPCreateCStringCopyFromNSString(content.title);
#else
    return nil;
#endif
}

NPBINDING DONTSTRIP void NPNotificationContentSetTitle(IntPtr contentPtr, const char* value)
{
#if !TARGET_OS_TV
    UNMutableNotificationContent*       content     = (__bridge UNMutableNotificationContent*)contentPtr;
    [content setTitle:NPCreateNSStringFromCString(value)];
#endif
}

NPBINDING DONTSTRIP const char* NPNotificationContentGetSubtitle(IntPtr contentPtr)
{
#if !TARGET_OS_TV
    UNNotificationContent*              content     = (__bridge UNNotificationContent*)contentPtr;
    return NPCreateCStringCopyFromNSString(content.subtitle);
#else
    return nil;
#endif
}

NPBINDING DONTSTRIP void NPNotificationContentSetSubtitle(IntPtr contentPtr, const char* value)
{
#if !TARGET_OS_TV
    UNMutableNotificationContent*       content     = (__bridge UNMutableNotificationContent*)contentPtr;
    [content setSubtitle:NPCreateNSStringFromCString(value)];
#endif
}

NPBINDING DONTSTRIP const char* NPNotificationContentGetBody(IntPtr contentPtr)
{
#if !TARGET_OS_TV
    UNNotificationContent*              content     = (__bridge UNNotificationContent*)contentPtr;
    return NPCreateCStringCopyFromNSString(content.body);
#else
    return nil;
#endif
}

NPBINDING DONTSTRIP void NPNotificationContentSetBody(IntPtr contentPtr, const char* value)
{
#if !TARGET_OS_TV
    UNMutableNotificationContent*       content     = (__bridge UNMutableNotificationContent*)contentPtr;
    [content setBody:NPCreateNSStringFromCString(value)];
#endif
}

NPBINDING DONTSTRIP int NPNotificationContentGetBadge(IntPtr contentPtr)
{
    UNNotificationContent*              content     = (__bridge UNNotificationContent*)contentPtr;
    return (int)[content.badge integerValue];
}

NPBINDING DONTSTRIP void NPNotificationContentSetBadge(IntPtr contentPtr, int value)
{
    UNMutableNotificationContent*       content     = (__bridge UNMutableNotificationContent*)contentPtr;
    [content setBadge:[NSNumber numberWithInteger:value]];
}

NPBINDING DONTSTRIP const char* NPNotificationContentGetUserInfo(IntPtr contentPtr)
{
    UNNotificationContent*              content     = (__bridge UNNotificationContent*)contentPtr;
    NSError*                            error;
    NSString*                           jsonStr     = NPToJson(content.userInfo, &error);
    return NPCreateCStringCopyFromNSString(jsonStr);
}

NPBINDING DONTSTRIP void NPNotificationContentSetUserInfo(IntPtr contentPtr, const char* jsonStr)
{
    UNMutableNotificationContent*       content     = (__bridge UNMutableNotificationContent*)contentPtr;
    NSError*                            error;
    [content setUserInfo:NPFromJson(NPCreateNSStringFromCString(jsonStr), &error)];
}

NPBINDING DONTSTRIP void NPNotificationContentSetSoundName(IntPtr contentPtr, const char* soundName)
{
    UNMutableNotificationContent*       content     = (__bridge UNMutableNotificationContent*)contentPtr;
    [content setSound:[UNNotificationSound soundNamed:NPCreateNSStringFromCString(soundName)]];
}

NPBINDING DONTSTRIP const char* NPNotificationContentGetLaunchImageName(IntPtr contentPtr)
{
#if !TARGET_OS_OSX || !TARGET_OS_TV
    UNNotificationContent*              content     = (__bridge UNNotificationContent*)contentPtr;
    return NPCreateCStringCopyFromNSString(content.launchImageName);
#else
    return nil;
#endif
}

NPBINDING DONTSTRIP void NPNotificationContentSetLaunchImageName(IntPtr contentPtr, const char* value)
{
#if !TARGET_OS_OSX || !TARGET_OS_TV
    UNMutableNotificationContent*       content     = (__bridge UNMutableNotificationContent*)contentPtr;
    [content setLaunchImageName:NPCreateNSStringFromCString(value)];
#endif
}

NPBINDING DONTSTRIP const char* NPNotificationContentGetCategoryId(IntPtr contentPtr)
{
#if !TARGET_OS_TV
    UNNotificationContent*              content     = (__bridge UNNotificationContent*)contentPtr;
    return NPCreateCStringCopyFromNSString(content.categoryIdentifier);
#else
    return nil;
#endif
}

NPBINDING DONTSTRIP bool NPNotificationTriggerGetRepeats(IntPtr triggerPtr)
{
    UNNotificationTrigger*              trigger     = (__bridge UNNotificationTrigger*)triggerPtr;
    return trigger.repeats;
}

NPBINDING DONTSTRIP IntPtr NPTimeIntervalNotificationTriggerCreate(double interval, bool repeats)
{
    UNTimeIntervalNotificationTrigger*  trigger     = [UNTimeIntervalNotificationTrigger triggerWithTimeInterval:interval repeats:repeats];
    return NPRetainWithOwnershipTransfer(trigger);
}

NPBINDING DONTSTRIP void NPTimeIntervalNotificationTriggerGetProperties(IntPtr triggerPtr, double* timeInterval, char* nextTriggerDate, bool* repeats)
{
    UNTimeIntervalNotificationTrigger*  trigger     = (__bridge UNTimeIntervalNotificationTrigger*)triggerPtr;
    
    // set values
    *timeInterval   = trigger.timeInterval;
    nextTriggerDate = NPCreateCStringFromNSString(NPCreateNSStringFromNSDate(trigger.nextTriggerDate));
    *repeats        = trigger.repeats;
}

NPBINDING DONTSTRIP IntPtr NPCalendarNotificationTriggerCreate(NPUnityDateComponents dateComponents, bool repeats)
{
    // create date component
    NSDateComponents*                   components  = dateComponents.ToNSDateComponents();
    
    // create trigger
    UNCalendarNotificationTrigger*      trigger     = [UNCalendarNotificationTrigger triggerWithDateMatchingComponents:components repeats:repeats];
    return NPRetainWithOwnershipTransfer(trigger);
}

NPBINDING DONTSTRIP void NPCalendarNotificationTriggerGetProperties(IntPtr triggerPtr, NPUnityDateComponents* dateComponents, char* nextTriggerDate, bool* repeats)
{
    UNCalendarNotificationTrigger*      trigger     = (__bridge UNCalendarNotificationTrigger*)triggerPtr;
    
    // set values
    dateComponents->CopyProperties(trigger.dateComponents);
    nextTriggerDate = NPCreateCStringFromNSString(NPCreateNSStringFromNSDate(trigger.nextTriggerDate));
    *repeats        = trigger.repeats;
}

NPBINDING DONTSTRIP IntPtr NPLocationNotificationTriggerCreate(NPUnityCircularRegion regionData, bool notifyOnEntry, bool notifyOnExit, bool repeats)
{
#if NATIVE_PLUGINS_USES_CORE_LOCATION
    // set region info
    CLLocationCoordinate2D              center      = CLLocationCoordinate2DMake(regionData.latitude, regionData.longitude);
    CLCircularRegion*                   region      = [[CLCircularRegion alloc] initWithCenter:center
                                                                                        radius:regionData.radius
                                                                                    identifier:NPCreateNSStringFromCString((const char*)regionData.regionIdPtr)];
    region.notifyOnEntry                            = notifyOnEntry;
    region.notifyOnExit                             = notifyOnExit;
    
    // create trigger
    UNLocationNotificationTrigger*      trigger     = [UNLocationNotificationTrigger triggerWithRegion:region repeats:repeats];
    return NPRetainWithOwnershipTransfer(trigger);
#else
    return nil;
#endif
}

NPBINDING DONTSTRIP void NPLocationNotificationTriggerGetProperties(IntPtr triggerPtr, NPUnityCircularRegion* regionData, bool* notifyOnEntry, bool* notifyOnExit, bool* repeats)
{
#if NATIVE_PLUGINS_USES_CORE_LOCATION
    UNLocationNotificationTrigger*      trigger     = (__bridge UNLocationNotificationTrigger*)triggerPtr;
    __weak CLCircularRegion*            region      = (CLCircularRegion*)trigger.region;
    
    // copy properties
    regionData->latitude        = region.center.latitude;
    regionData->longitude       = region.center.longitude;
    regionData->radius          = region.radius;
    regionData->regionIdPtr     = NPCreateCStringFromNSString(region.identifier);
    *notifyOnEntry              = region.notifyOnEntry;
    *notifyOnExit               = region.notifyOnExit;
    *repeats                    = trigger.repeats;
#endif
}
