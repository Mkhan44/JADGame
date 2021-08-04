using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public enum NotificationServicesDemoActionType
	{
		RequestAccess,
		GetSettings,
        CreateNotification,
        ScheduleNotificationNow,
        ScheduleNotification,
        GetScheduledNotifications,
        CancelScheduledNotification,
        CancelAllScheduledNotifications,
        GetDeliveredNotifications,
        RemoveAllDeliveredNotifications,
        DeviceToken,
        RegisterForRemoteNotifications,
        UnregisterForRemoteNotifications,
        IsRegisteredForRemoteNotifications,
		ResourcePage,
	}

	public class NotificationServicesDemoAction : DemoActionBehaviour<NotificationServicesDemoActionType> 
	{}
}