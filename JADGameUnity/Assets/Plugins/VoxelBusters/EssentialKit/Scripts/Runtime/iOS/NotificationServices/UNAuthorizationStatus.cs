#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{
    internal enum UNAuthorizationStatus : long
    {
        // The user has not yet made a choice regarding whether the application may post user notifications.
        UNAuthorizationStatusNotDetermined = 0,
    
        // The application is not authorized to post user notifications.
        UNAuthorizationStatusDenied,
        
        // The application is authorized to post user notifications.
        UNAuthorizationStatusAuthorized,
        
        // The application is authorized to post non-interruptive user notifications.
        UNAuthorizationStatusProvisional,
    }
}
#endif