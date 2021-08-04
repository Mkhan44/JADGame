#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.NotificationServicesCore.iOS
{
    internal enum UNNotificationSetting : long
    {
        // The application does not support this notification type
        UNNotificationSettingNotSupported  = 0,
    
        // The notification setting is turned off.
        UNNotificationSettingDisabled,
        
        // The notification setting is turned on.
        UNNotificationSettingEnabled,
    }
}
#endif